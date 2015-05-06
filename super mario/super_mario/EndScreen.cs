using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    class EndScreen : GameScreen
    {
        Texture2D background;
        SpriteFont font1;
        MenuManager menu;
        string text;
        bool win;

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            background = this.content.Load<Texture2D>("background");
            font1 = this.content.Load<SpriteFont>("Fonts/Font1");
            win = false;
            Camera.Instance.SetCameraPoint(new Vector2(0, 0));

            if (Player.Instance.flagUp == true && Player.Instance.InCastle == true)
                text = "Level cleared";
            else
                text = "You died";

            menu = new MenuManager();
            menu.LoadContent(content, "End");
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            if (Player.Instance.InCastle == true && Player.Instance.flagUp)
            {
                Win();
                menu.Update(gameTime, inputManager);
            }
            else
            {
                Lose();
                menu.Update(gameTime, inputManager);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            if (win == true)
            {
                menu.Draw(spriteBatch);
                spriteBatch.DrawString(font1, text, new Vector2(50, 20), Color.White);
                spriteBatch.DrawString(font1, "Score: " + Player.Instance.score.ToString(), new Vector2(50, 50), Color.White);
            }
            else
            {
                menu.Draw(spriteBatch);
                spriteBatch.DrawString(font1, text, new Vector2(50, 20), Color.White);
                
            }
        }

        private void Win()
        {
            win = true;
        }

        private void Lose()
        {
        }
    }
}
