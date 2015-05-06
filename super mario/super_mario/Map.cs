using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class Map
    {
        public Layer layer1;
        public Layer layer2;
        string id;

        public string ID
        {
            get { return id; }
        }

        public void LoadContent(ContentManager content, Map map, string mapID)
        {
            layer1 = new Layer();
            layer2 = new Layer();
            id = mapID;

            layer1.LoadContent(map, "Layer1");
            layer2.LoadContent(map, "Layer2");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime, Player player)
        {
            layer1.Update(gameTime, player);
            layer2.Update(gameTime, player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            layer1.Draw(spriteBatch);
            layer2.Draw(spriteBatch);
        }
    }
}
