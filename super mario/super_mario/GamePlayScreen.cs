﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace super_mario
{
    class GamePlayScreen : GameScreen
    {
        SoundEffect musicTheme, timeWarning;
        SoundEffectInstance musicYes;
        Player player;
        Map map;
        Layer layer;
        SpriteFont font;
        Vector2 timerPos;
        EnemyHandler enemyHandler;

        float gameTimer = 33f;
        float overworldTimer = 0f;
        float mpTimer = 0f;
        bool playedT = false;
        bool musicPaused = true;
        bool timePointsAdded;

        //End 
        float loseTimer;

        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            enemyHandler = new EnemyHandler();
            enemyHandler.LoadContent(content, input);
            musicTheme = this.content.Load<SoundEffect>("Sound/overworld");
            musicYes = musicTheme.CreateInstance();
            timeWarning = this.content.Load<SoundEffect>("Sound/timeWarning");
            player = new Player();
            map = new Map();
            layer = new Layer();
            map.LoadContent(content, map, "Map1");
            player.LoadContent(content, input);
            font = this.content.Load<SpriteFont>("Fonts/timeFont");
            timePointsAdded = false;
            loseTimer = 0;

            LoadObjects(content);
        }

        public void LoadObjects(ContentManager objectLoader)
        {
            ObjectHandler.mushroomImage = objectLoader.Load<Texture2D>("Objects/mushroom");
            ObjectHandler.starImage = objectLoader.Load<Texture2D>("Objects/star");
            ObjectHandler.pFImage = objectLoader.Load<Texture2D>("Objects/poleflag");
            ObjectHandler.cFImage = objectLoader.Load<Texture2D>("Objects/castleflag");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            map.UnloadContent();
            ObjectHandler.instance.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (ObjectHandler.cFImage == null || ObjectHandler.pFImage == null)
            {
                LoadObjects(content);
            }

            if (player.castle == true && timePointsAdded == false)
            {
                int gtP = (int)Convert.ToInt64(gameTimer);
                player.score += gtP * 3;
                timePointsAdded = true;
            }
            if (player.end == true)
            {
                loseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (loseTimer >= 2)
                {
                    ScreenManager.Instance.AddScreen(new EndScreen(), inputManager);
                }
            }

            if(player.flagUp == true && timePointsAdded == true)
            {
                ScreenManager.Instance.AddScreen(new EndScreen(), inputManager); 
            }

            if (inputManager.KeyPressed(Keys.Escape))
            {
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }

            if (player.transition == false)
                inputManager.Update();

            SoundEffect.MasterVolume = OptionsScreen.masterVolume;

            //Music
            if (overworldTimer == 0f && musicPaused == true || musicPaused == true && playedT == true)
            {
                musicYes.Play();
                musicPaused = false;
            }

            

            overworldTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (overworldTimer >= 304)
                overworldTimer = 0f;

            if (gameTimer >= 0 && player.onFlag == false)
                gameTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (gameTimer == 0)
                player.Health = 0;

            

            timerPos = new Vector2(Camera.Instance.Position.X + 5, 5);

            if (gameTimer <= 30 && playedT == false)
            {
                timeWarning.Play();
                musicYes.Pause();
                playedT = true;
            }
            if (playedT == true)
            {
                mpTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (mpTimer >= 2)
                {
                    musicPaused = true;
                }
            }

            if (player.scPlayed == true)
            {
                musicYes.Stop();
            }

            if (player.transition == false && player.end == false)
            {
                map.Update(gameTime, player);
                enemyHandler.Update(gameTime, player);
            }

            if (inputManager.KeyPressed(Keys.O))
            {
                musicYes.Pause();
                ScreenManager.Instance.AddScreenKeep(new OptionsScreen(), inputManager);
            }
            if (player.end == false)
            {
                player.Update(gameTime, inputManager, map.layer1);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            ObjectHandler.instance.Draw(spriteBatch);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            enemyHandler.Draw(spriteBatch);
            int gt = (int)Convert.ToInt64(gameTimer);
            spriteBatch.DrawString(font, gt.ToString(), timerPos, Color.White);
        }
    }
}