using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class Star : Entity
    {
        bool left, right, spawned, removed, up, down;
        public bool bounce;
        float timer, dirTimer;

        public bool Spawned
        {
            get { return spawned; }
            set { spawned = value; }
        }

        public bool Removed
        {
            get { return removed; }
        }

        public Star(Vector2 pos, bool spawned)
        {
            this.position = pos;
            this.spawned = spawned;
            bounce = false;
        }

        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            this.image = this.content.Load<Texture2D>("Objects/star");
            moveAnimation = new SpriteAnimation();
            moveAnimation.Frames = new Vector2(image.Width / 16, image.Height / 16);
            moveAnimation.LoadContent(content, image, "", position);

            syncTilePosition = false;
            activateGravity = true;
            removed = false;
        }

        public override void Update(GameTime gameTime, Player player)
        {
            moveAnimation.IsActive = true;
            moveAnimation.Position = position;
            moveAnimation.Update(gameTime);

            syncTilePosition = false;
            prevPos = position;
            if (spawned == false)
            {
                velocity.Y = -1;

                position += velocity;
                
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer >= 266)
                {
                    
                    spawned = true;
                    activateGravity = true;
                    velocity = new Vector2(0, 0);
                    gravity = 10f;

                    if (right == true)
                    {
                        velocity.X = -1;
                    }
                    else if (left == true)
                    {
                        velocity.X = 1;
                    }
                }

                if (left == false && right == false)
                {
                    if (player.Position.X + 8 > position.X)
                        left = true;
                    else if (player.Position.X + 8 <= position.X)
                        right = true;
                }

                
            }
            else if (spawned == true)
            {
                dirTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (velocity.Y > 0)
                    down = true;
                else if (velocity.Y < 0)
                    up = true;

                if (velocity.Y == 0)
                    velocity.Y = -1f;

                if (up == true)
                {
                    if (dirTimer >= 800)
                    {
                        velocity.Y = 1f;
                        up = false;
                        down = true;
                        dirTimer = 0;
                    }
                }

                if (this.SRect.Intersects(player.SRect))
                {
                    player.starMan = true;
                    removed = true;
                    player.transition = true;
                }


                if (bounce == true)
                {
                    if (up == true)
                    {
                        down = true;
                        velocity.Y = -1f;
                        bounce = false;
                    }
                    else if (down == true)
                    {
                        up = true;
                        velocity.Y = 1f;
                        bounce = false;
                    }
                }

                position += velocity;
            }



            if (position.X - player.Position.X >= 140 || player.Position.X - player.Position.X <= -140)
                removed = true;
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
        }
    }
}
