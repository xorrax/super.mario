using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class Enemy : Entity
    {

        public bool dead = false;
        bool stomped = false;
        float moveTimer, stompedTimer;
        bool direction;
        float speed = 0.5f;
        SoundEffect stomp;


        public Enemy(Vector2 pos)
        {
            this.position = pos;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            fileManager = new FileManager();
            moveAnimation = new SpriteAnimation();
            Vector2 tempFrames = Vector2.Zero;
            moveSpeed = 10f;
            stomp = this.content.Load<SoundEffect>("Sound/stomp");

            fileManager.LoadContent("Load/Enemy.cme", attributes, contents);
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch(attributes[i][j])
                    {
                        case"Image":
                            image = this.content.Load<Texture2D>(contents[i][j]);
                            break;
                    }
                }
            }

            moveAnimation.Frames = new Vector2(image.Width / 16, image.Height / 16);
            moveAnimation.LoadContent(content, image, "", position);
        }
        
        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, Player player)
        {
            prevPos = position;
            moveAnimation.IsActive = true;
            if (moveTimer >= 4)
            {
                direction = !direction;
                moveTimer = 0f;
            }

            moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Stomp
            if (stomped == false)
            {
                if (direction == false)
                {
                    position.X += speed;
                }
                else if (direction == true)
                {
                    position.X -= speed;
                }
            }

            //Collision
            FloatRect prevSPlayer = new FloatRect(player.PrevPosition.X, player.PrevPosition.Y, 16, 16);
            FloatRect prevBPlayer = new FloatRect(player.PrevPosition.X, player.PrevPosition.Y, 16, 32);
            FloatRect prevEnemy = new FloatRect(PrevPosition.X, PrevPosition.Y, 16, 16);
            if (player.starMan == false)
            {
                if (player.bigMario == false)
                {
                    if (player.SRect.Intersects(this.SRect) && stomped == false)
                    {
                        if (player.SRect.Bottom >= SRect.Top && prevSPlayer.Bottom <= prevEnemy.Top && player.Hit == false)
                        {
                            stomped = true;
                            stomp.Play();
                            player.Velocity = new Vector2(player.Velocity.X, -3);
                            player.score += 100;
                        }
                        else if (player.SRect.Left <= this.SRect.Left && prevSPlayer.Left <= prevEnemy.Left)
                        {
                            player.Position = new Vector2(prevSPlayer.Left, player.Position.Y - velocity.Y);
                            if(player.Hit == false)
                                player.Health = 0;
                            player.Hit = true;
                        }
                        else if (player.SRect.Right >= this.SRect.Right && prevSPlayer.Right >= prevEnemy.Right)
                        {
                            player.Position = new Vector2(prevSPlayer.Left, player.Position.Y - velocity.Y);
                            if(player.Hit == false)
                                player.Health = 0;
                            player.Hit = true;
                        }

                    }
                }
                else if (player.bigMario == true)
                {
                    if (player.BRect.Intersects(this.SRect) && stomped == false)
                    {
                        if (player.BRect.Bottom >= SRect.Top && prevBPlayer.Bottom <= prevEnemy.Top && player.Hit == false)
                        {
                            stomped = true;
                            stomp.Play();
                            player.Velocity = new Vector2(player.Velocity.X, -3);
                            player.score += 100;
                        }
                        else if (player.BRect.Left <= this.SRect.Left && prevBPlayer.Left <= prevEnemy.Left)
                        {
                            player.Position = new Vector2(prevBPlayer.Left, player.Position.Y);
                            if(player.Hit == false)
                                player.transition = true;

                            player.Hit = true;
                        }
                        else if (player.BRect.Right >= this.SRect.Right && prevBPlayer.Right >= prevEnemy.Right)
                        {
                            player.Position = new Vector2(prevBPlayer.Left, player.Position.Y);
                            if(player.Hit == false)
                                player.transition = true;

                            player.Hit = true;
                        }
                    }
                }

            }
            else if (player.starMan)
            {
                if (player.bigMario == true)
                {
                    if (player.BRect.Intersects(this.SRect) && stomped == false)
                    {
                        stomped = true;
                        if (player.BRect.Bottom >= SRect.Top && prevBPlayer.Bottom <= prevEnemy.Top)
                        {
                            stomped = true;
                            stomp.Play();
                            player.Velocity = new Vector2(player.Velocity.X, -3);
                            player.score += 100;
                        }
                        else if (player.BRect.Left <= this.SRect.Left && prevBPlayer.Left <= prevEnemy.Left)
                        {
                            stomp.Play();
                            stomped = true;
                        }
                        else if (player.BRect.Right >= this.SRect.Right && prevBPlayer.Right >= prevEnemy.Right)
                        {
                            stomp.Play();
                            stomped = true;
                        }
                    }
                }
                else if (player.bigMario == false)
                {
                    if (player.SRect.Intersects(this.SRect))
                    {
                        if (player.SRect.Bottom >= SRect.Top && prevSPlayer.Bottom <= prevEnemy.Top)
                        {
                            stomped = true;
                            stomp.Play();
                            player.Velocity = new Vector2(player.Velocity.X, -3);
                            player.score += 100;
                        }
                        else if (player.SRect.Left <= this.SRect.Left && prevSPlayer.Left <= prevEnemy.Left)
                        {
                            stomp.Play();
                            player.Position = new Vector2(prevSPlayer.Left, player.Position.Y - velocity.Y);
                            stomped = true;
                        }
                        else if (player.SRect.Right >= this.SRect.Right && prevSPlayer.Right >= prevEnemy.Right)
                        {
                            stomp.Play();
                            player.Position = new Vector2(prevSPlayer.Left, player.Position.Y - velocity.Y);
                            stomped = true;
                        }
                    }
                }
            }


            if (stomped == true)
            {
                stompedTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
            }

            if (stompedTimer >= 0.2f)
            {
                dead = true;
                stompedTimer = 0f;
            }

            moveAnimation.Position = position;
            moveAnimation.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
        }
    }
}
