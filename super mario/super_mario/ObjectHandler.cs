using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class ObjectHandler
    {
        public List<Mushroom> mushroomList = new List<Mushroom>();
        public List<Star> starList = new List<Star>();

        public PoleFlag pF = new PoleFlag(Vector2.Zero, null);
        public CastleFlag cF = new CastleFlag(Vector2.Zero, null);

        public static Texture2D mushroomImage;
        public static Texture2D starImage;
        public static Texture2D pFImage;
        public static Texture2D cFImage;

        public static ObjectHandler instance = new ObjectHandler();

        Vector2 mushroomPos, cFPos, pFPos, starPos;

        bool spawnMushroom, spawnStar, spawnpF;
        bool spawncF = true;
        public bool unloading = false;

        bool a = false;
        bool b = false;

        public Vector2 MushroomPos
        {
            set { mushroomPos = value; }
        }

        public Vector2 StarPos
        {
            set { starPos = value; }
        }

        public Vector2 CFPos
        {
            set { cFPos = value; }
        }

        public Vector2 PFPos
        {
            set { pFPos = value; }
        }

        public bool SpawnMushroom
        {
            set { spawnMushroom = value; }
        }

        public bool SpawnStar
        {
            set { spawnStar = value; }
        }

        public bool SpawnpF
        {
            set { spawnpF = value; }
        }

        public bool SpawncF
        {
            set { spawncF = value; }
        }

        public void UnloadContent()
        {
            unloading = true;
            pF.Image = null;
            cF.Image = null;
            mushroomImage = null;
            starImage = null;
            mushroomList = new List<Mushroom>();
            starList = new List<Star>();
            spawnpF = true;
            spawncF = true;
            pF = new PoleFlag(Vector2.Zero, null);
            cF = new CastleFlag(Vector2.Zero, null);
        }

        public void Update(GameTime gameTime, Player player, ContentManager content, InputManager input)
        {
            instance = this;

            if (a == false)
            {
                spawnpF = true;
                a = true;
            }

            if (b == false)
            {
                spawncF = true;
                b = true;
            }

            if (spawnMushroom == true)
            {
                mushroomList.Add(new Mushroom(mushroomPos, mushroomImage, false));
                spawnMushroom = false;
            }

            if(spawnStar == true)
            {
                starList.Add(new Star(starPos, false));

                foreach (Star s in starList)
                {
                    s.LoadContent(content, input);
                }

                spawnStar = false;
            }

            if (spawncF == true && player.InCastle == true)
            {
                cF = new CastleFlag(cFPos, cFImage);
                spawncF = false;
            }

            if (spawnpF == true)
            {
                pF = new PoleFlag(pFPos, pFImage);
                spawnpF = false;
            }

            if (player.onFlag == true)
                pF.a = true;

            if (pF.flagDown == true)
                player.flagDown = true;

            if (cF.flagUp == true)
            {
                player.flagUp = true;
            }

            foreach (Mushroom m in mushroomList)
                m.Update(gameTime, player);

            foreach (Star s in starList)
                s.Update(gameTime, player);

            pF.Update();
            cF.Update();
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Mushroom m in mushroomList)
            {
                m.Draw(spriteBatch);
            }

            foreach (Star s in starList)
            {
                s.Draw(spriteBatch);
            }

           
            if(cF.Image != null && cF.Position != Vector2.Zero)
                spriteBatch.Draw(cF.Image, cF.Position, Color.White);

            if(pF.Image != null && pF.Position != Vector2.Zero)
                spriteBatch.Draw(pF.Image, pF.Position, Color.White);
        }
    }
}
