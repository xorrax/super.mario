using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class Tile
    {
        public enum State { Solid, Passive };
        public enum Motion { Static, Horizontal, Vertical };
        public enum Specials { Destroyable, Mushroom, Normal, Flag, FlagB, Flagtop, CastleFlag, Star, Coin, Castle};

        public State state;
        public Motion motion;
        public Specials specials;
        Vector2 position, prevPosition, velocity;
        Texture2D tileImage;
        public FloatRect rect = new FloatRect(0, 0, 0, 0);
        FloatRect prevTile = new FloatRect(0, 0, 0, 0);

        float range, moveSpeed;
        int counter;
        bool increase, playerOnTile, mushroomOnTile;
        public bool setPos;
        public bool destroyed = false;
        public bool activated = false;
        bool col, prevCol;
        Animation animation;

        public Vector2 Position
        {
            get { return position; }
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)tileImage.Width, (int)tileImage.Height);
        }

        private Texture2D CropImage(Texture2D tileSheet, Rectangle tileArea)
        {
            Texture2D croppedImage = new Texture2D(tileSheet.GraphicsDevice, tileArea.Width, tileArea.Height);

            Color[] tileSheetData = new Color[tileSheet.Width * tileSheet.Height];
            Color[] croppedImageData = new Color[croppedImage.Width * croppedImage.Height];

            tileSheet.GetData<Color>(tileSheetData);

            int index = 0;
            for (int y = tileArea.Y; y < tileArea.Y + tileArea.Height; y++)
            {
                for (int x = tileArea.X; x < tileArea.X + tileArea.Width; x++)
                {
                    croppedImageData[index] = tileSheetData[y * tileSheet.Width + x];
                    index++;
                }
            }

            croppedImage.SetData<Color>(croppedImageData);

            return croppedImage;
        }

        public void SetTile(State state, Motion motion, Specials specials, Vector2 position, Texture2D tileSheet, Rectangle tileArea, float MoveSpeed)
        {
            this.state = state;
            this.motion = motion;
            this.specials = specials;
            this.position = position;
            increase = true;

            tileImage = CropImage(tileSheet, tileArea);
            range = 80;
            counter = 0;
            moveSpeed = MoveSpeed;
            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, "", position);
            playerOnTile = false;
            mushroomOnTile = false;
            velocity = Vector2.Zero;
            setPos = false;
            col = false;
        }
        public void Update(GameTime gameTime, Player player, Layer layer, ObjectHandler oh)
        {
            counter++;
            prevPosition = position;
            prevCol = col;
            col = false;

            if (counter >= range)
            {
                counter = 0;
                increase = !increase;
            }

            if (motion == Motion.Horizontal)
            {
                if (increase)
                    velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (motion == Motion.Vertical)
            {
                if (increase)
                    velocity.Y = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.Y = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            position += velocity;
            animation.Position = position;
            rect = new FloatRect(position.X, position.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);
            FloatRect prevPlayer = new FloatRect(player.PrevPosition.X, player.PrevPosition.Y, 16, 16);

            prevTile = new FloatRect(prevPosition.X, prevPosition.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

            if (player.tileCol == true)
            {
                player.tileTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (player.tileTimer >= 6f)
                {
                    player.tileCol = false;
                    player.tileTimer = 0f;
                }
            }

            if (player.tileCol == false)
            {
                if (player.bigMario == false)
                {
                    if (player.SRect.Intersects(rect) && state == State.Solid && specials == Specials.Mushroom && player.PrevPosition.Y > position.Y && player.Velocity.Y < 0 && prevPlayer.Top >= prevTile.Bottom)
                    {
                        activated = true;
                        player.Velocity = new Vector2(player.Velocity.X, 0);
                        player.tileCol = true;
                    }

                    if (player.SRect.Intersects(rect) && state == State.Solid && specials == Specials.Star && player.PrevPosition.Y > position.Y && player.Velocity.Y < 0 && prevPlayer.Top >= prevTile.Bottom)
                    {
                        activated = true;
                        player.Velocity = new Vector2(player.Velocity.X, 0);
                        player.tileCol = true;
                    }

                    //if (player.SRect.Intersects(rect) && this.state == State.Solid && this.specials == Specials.Destroyable && player.Position.Y > position.Y && player.Velocity.Y < 0)
                    //{
                    //    destroyed = true;
                    //    player.Velocity = new Vector2(player.Velocity.X, 0);
                    //}
                    if (specials == Specials.Flag)
                    {
                        FloatRect flagRect = new FloatRect(position.X + 7, position.Y, 2, 16);

                        if (player.SRect.Intersects(flagRect) && this.specials == Specials.Flag)
                        {
                            if (player.setPos == false)
                            {
                                player.Position = new Vector2(this.position.X - 5, player.Position.Y);
                                player.setPos = true;
                            }
                            player.onFlag = true;
                        }
                    }
                }
                else if (player.bigMario == true)
                {
                    if (player.BRect.Intersects(rect) && state == State.Solid && specials == Specials.Mushroom && player.PrevPosition.Y > position.Y && player.Velocity.Y < 0)
                    {
                        activated = true;
                        player.Velocity = new Vector2(player.Velocity.X, 0);
                        player.tileCol = true;
                    }

                    if (player.BRect.Intersects(rect) && this.state == State.Solid && this.specials == Specials.Destroyable && player.Position.Y > position.Y && player.Velocity.Y < 0)
                    {
                        destroyed = true;
                        player.Velocity = new Vector2(player.Velocity.X, 0);
                    }
                    if (player.BRect.Intersects(rect) && state == State.Solid && specials == Specials.Star && player.PrevPosition.Y > position.Y && player.Velocity.Y < 0 && prevPlayer.Top >= prevTile.Bottom)
                    {
                        activated = true;
                        player.Velocity = new Vector2(player.Velocity.X, 0);
                        player.tileCol = true;
                    }
                    if (specials == Specials.Flag)
                    {
                        FloatRect flagRect = new FloatRect(position.X + 7, position.Y, 2, 16);

                        if (player.BRect.Intersects(flagRect))
                        {
                            if (player.setPos == false)
                            {
                                player.Position = new Vector2(this.position.X - 5, player.Position.Y);
                                player.setPos = true;
                            }
                            player.onFlag = true;
                        }
                    }
                }
            }

            if (player.SRect.Intersects(rect) && this.specials == Specials.FlagB && player.setPos == true)
            {
                player.flagB = true;
            }


            
            //Castle
            //if (player.bigMario == true)
            //{
            //    if (player.BRect.Intersects(rect) && specials == Specials.Castle)
            //    {
            //        player.gameWin = true;
            //    }
            //}
            //else if (player.bigMario == false)
            //{
            //    if (player.SRect.Intersects(rect))
            //    {
            //        player.gameWin = true;
            //    }
            //}

            //Mushroom intersections
            for (int i = 0; i < oh.mushroomList.Count; i++)
            {
                FloatRect prevMush = new FloatRect(oh.mushroomList[i].PrevPosition.X, oh.mushroomList[i].PrevPosition.Y, 16, 16);
                if (mushroomOnTile)
                {
                    if (!oh.mushroomList[i].SyncTilePosition)
                    {
                        oh.mushroomList[i].Position += velocity;
                        oh.mushroomList[i].SyncTilePosition = true;
                    }

                    if (oh.mushroomList[i].SRect.Right < rect.Left || oh.mushroomList[i].SRect.Left > rect.Right || oh.mushroomList[i].SRect.Bottom != rect.Top)
                    {
                        mushroomOnTile = false;
                        oh.mushroomList[i].ActivateGravity = true;
                    }
                }
                if (oh.mushroomList[i].Spawned == true)
                {
                    if (oh.mushroomList[i].SRect.Intersects(rect) && state == State.Solid)
                    {
                        if (oh.mushroomList[i].SRect.Bottom >= rect.Top && prevMush.Bottom <= prevTile.Top)
                        {
                            oh.mushroomList[i].Position = new Vector2(oh.mushroomList[i].Position.X, position.Y - 16);
                            oh.mushroomList[i].ActivateGravity = false;
                            mushroomOnTile = true;
                        }
                        else if (oh.mushroomList[i].SRect.Top <= rect.Bottom && prevMush.Top >= prevTile.Bottom)
                        {
                            oh.mushroomList[i].Position = new Vector2(oh.mushroomList[i].Position.X, position.Y + Layer.TileDimensions.Y);
                            oh.mushroomList[i].Velocity = new Vector2(oh.mushroomList[i].Velocity.X, 0);
                            oh.mushroomList[i].ActivateGravity = true;
                        }
                        else
                        {
                            oh.mushroomList[i].Position -= oh.mushroomList[i].Velocity;
                        }
                    }

                }
            }
            //--------

            //Star intersection
            for (int i = 0; i < oh.starList.Count; i++)
            {
                FloatRect prevStar = new FloatRect(oh.starList[i].PrevPosition.X, oh.starList[i].PrevPosition.Y, 16, 16);

                if (oh.starList[i].SRect.Intersects(rect) && state == State.Solid)
                {
                    if (oh.starList[i].SRect.Bottom >= rect.Top && prevStar.Top >= prevTile.Bottom)
                        oh.starList[i].Velocity = new Vector2(oh.starList[i].Velocity.X, 0);
                    {
                        oh.starList[i].bounce = true;
                    }
                }

                //if (starOnTile)
                //{
                //    if (!oh.starList[i].SyncTilePosition)
                //    {
                //        oh.starList[i].Position += velocity;
                //        oh.starList[i].SyncTilePosition = true;
                //    }

                //    if (oh.starList[i].SRect.Right < rect.Left || oh.starList[i].SRect.Left > rect.Right || oh.starList[i].SRect.Bottom != rect.Top)
                //    {
                //        starOnTile = false;
                //        oh.starList[i].ActivateGravity = true;
                //    }
                //}

                //if (oh.starList[i].Spawned == true)
                //{
                //    if (oh.starList[i].SRect.Intersects(rect) && state == State.Solid)
                //    {
                //        if (oh.starList[i].SRect.Bottom >= rect.Top && prevStar.Bottom <= prevTile.Top)
                //        {
                //            oh.starList[i].Position = new Vector2(oh.starList[i].Position.X, position.Y - 16);
                //            oh.starList[i].ActivateGravity = false;
                //            starOnTile = true;
                //        }
                //        else if (oh.starList[i].SRect.Top <= rect.Bottom && prevStar.Top >= prevTile.Bottom)
                //        {
                //            oh.starList[i].Position = new Vector2(oh.starList[i].Position.X, position.Y + Layer.TileDimensions.Y);
                //            oh.starList[i].Velocity = new Vector2(oh.starList[i].Velocity.X, 0);
                //            oh.starList[i].ActivateGravity = true;
                //        }
                //        else
                //        {
                //            oh.starList[i].Position -= oh.starList[i].Velocity;
                //        }
                //    }
                //}


            }

            //------
            if (player.bigMario == false)
            {
                if (player.SRect.Intersects(rect) && state == State.Solid)
                {
                    if (player.SRect.Bottom >= rect.Top && prevPlayer.Bottom <= prevTile.Top)
                    {
                        player.jumpingRight = false;
                        player.jumpingLeft = false;
                        player.jumping = true;
                        player.Position = new Vector2(player.Position.X, position.Y - 16);
                        player.ActivateGravity = false;

                        playerOnTile = true;
                    }
                    else if (player.SRect.Top <= rect.Bottom && prevPlayer.Top >= prevTile.Bottom)
                    {
                        player.Position = new Vector2(player.Position.X, position.Y + Layer.TileDimensions.Y);
                        player.Velocity = new Vector2(player.Velocity.X, 0);
                        player.ActivateGravity = true;
                    }
                    else if (player.SRect.Left <= rect.Left && prevPlayer.Left <= prevTile.Left)
                    {
                        player.Position = new Vector2(prevPlayer.Left, player.Position.Y - velocity.Y);
                        player.ActivateGravity = true;
                    }
                    else if (player.SRect.Right >= rect.Right && prevPlayer.Right >= prevTile.Right)
                    {
                        player.Position = new Vector2(prevPlayer.Left, player.Position.Y - velocity.Y);
                        player.ActivateGravity = true;
                    }
                    else
                    {
                        player.Position -= player.Velocity;
                    }
                    
                }
        
            }
            else if (player.bigMario == true)
            {
                if (player.BRect.Intersects(rect) && state == State.Solid)
                {
                    if (player.BRect.Bottom + 16 >= rect.Top && prevPlayer.Bottom + 16 <= prevTile.Top)
                    {
                        player.jumpingRight = false;
                        player.jumpingLeft = false;
                        player.jumping = true;
                        player.Position = new Vector2(player.Position.X, position.Y - 32);
                        player.ActivateGravity = false;
                        playerOnTile = true;
                    }
                    else if (player.BRect.Top <= rect.Bottom && prevPlayer.Top >= prevTile.Bottom)
                    {
                        player.Position = new Vector2(player.Position.X, position.Y + Layer.TileDimensions.Y);
                        player.Velocity = new Vector2(player.Velocity.X, 0);
                        player.ActivateGravity = true;

                    }
                    else if (player.BRect.Left <= rect.Left && prevPlayer.Left <= prevTile.Left)
                    {
                        player.Position = new Vector2(prevPlayer.Left, player.Position.Y - velocity.Y);
                        player.ActivateGravity = true;

                    }
                    else if (player.BRect.Right >= rect.Right && prevPlayer.Right >= prevTile.Right)
                    {
                        player.Position = new Vector2(prevPlayer.Left, player.Position.Y - velocity.Y);
                        player.ActivateGravity = true;
                    }
                    else
                    {
                        player.Position -= player.Velocity;
                    }
                }
            }

            if (playerOnTile)
            {
                if (!player.SyncTilePosition)
                {
                    player.Position += velocity;
                    player.SyncTilePosition = true;
                }

                if (player.bigMario == false)
                {
                    if (player.SRect.Right < rect.Left || player.SRect.Left > rect.Right || player.SRect.Bottom != rect.Top)
                    {

                        playerOnTile = false;
                        player.ActivateGravity = true;
                    }
                }
                else if (player.bigMario == true)
                {
                    if (player.BRect.Right < rect.Left || player.BRect.Left > rect.Right || player.BRect.Bottom != rect.Top)
                    {
                        playerOnTile = false;
                        player.ActivateGravity = true;
                    }
                }
            }

            //Coin
            if (player.bigMario == true)
            {
                if (player.BRect.Intersects(rect) && specials == Specials.Coin)
                {
                    destroyed = true;
                    player.score += 100;
                }
            }
            else if (player.bigMario == false)
            {
                if (player.SRect.Intersects(rect) && specials == Specials.Coin)
                {
                    destroyed = true;
                    player.score += 100;
                }
            }
            
            player.Animation.Position = player.Position;
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
