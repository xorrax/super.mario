using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class Entity
    {
        protected int health;
        protected SpriteAnimation moveAnimation;
        protected float moveSpeed, gravity;

        protected ContentManager content;
        protected FileManager fileManager;

        protected Texture2D image;
        protected Vector2 position, velocity, prevPos;
        public ObjectHandler oh;

        protected List<List<string>> attributes, contents;

        public bool activateGravity, syncTilePosition;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public FloatRect SRect
        {
            get { return new FloatRect(position.X, position.Y, 16, 16); }
        }

        public FloatRect BRect
        {
            get { return new FloatRect(position.X, position.Y + 4, 16, 28); }
        }

        public Vector2 PrevPosition
        {
            get { return prevPos; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool ActivateGravity
        {
            set { activateGravity = value; }
        }

        public bool SyncTilePosition
        {
            get { return syncTilePosition; }
            set { syncTilePosition = value; }
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, 16, 16);
        }

        public Animation Animation
        {
            get { return moveAnimation; }
        }

        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }
        public virtual void LoadContent(ContentManager content, InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            oh = new ObjectHandler();
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime, InputManager input, Layer layer)
        {

        }

        public virtual void Update(GameTime gameTime, Player player)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
