using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace super_mario
{
    public class Mushroom : Entity
    {

        bool left, right, spawned, removed;
        float timer;

        public bool Removed
        {
            get { return removed; }
        }

        public bool Spawned
        {
            get { return spawned; }
            set { spawned = value; }
        }

        public Mushroom(Vector2 pos, Texture2D image, bool spawned)
        {
            this.position = pos;
            this.image = image;
            this.spawned = spawned;
        }

        public override void Update(GameTime gameTime, Player player)
        {
            syncTilePosition = false;
            prevPos = position;
            if(spawned == false)
            {
                velocity.Y = -1;
                position += velocity;
                

                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer >= 266)
                {
                    spawned = true;
                    activateGravity = true;
                    velocity = new Vector2(0,0);
                    gravity = 10f;

                    if (right == true)
                        velocity.X = -1;
                    if (left == true)
                        velocity.X = 1;
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
                if(this.SRect.Intersects(player.SRect))
                {
                    player.bigMario = true;
                    removed = true;
                    player.transition = true;
                }

                

                if (activateGravity)
                    velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds / 4;
                else
                    velocity.Y = 0;

                position += velocity;
            }

            if (Vector2.Distance(player.Position, this.Position) >= 130)
                removed = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.image, this.position, Color.White);
        }
    }
    
}
