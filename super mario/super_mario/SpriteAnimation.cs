using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class SpriteAnimation : Animation
    {

        int frameCounter, switchFrame;

        Vector2 frames, currentFrame;

        public Vector2 Frames
        {
            set { frames = value; }
        }

        public Vector2 CurrentFrame
        {
            set { currentFrame = value; }
            get { return currentFrame; }
        }

        public int FrameWidth
        {
            get { return image.Width / (int)frames.X; }
        }
        public int FrameHeight
        {
            get { return image.Height / (int)frames.Y; }
        }
        public override void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position, Player player)
        {
            base.LoadContent(Content, image, text, position);
            frameCounter = 0;
            switchFrame = 100;
            if (player.bigMario == false)
            {
                frames = new Vector2(image.Width / 16, image.Height / 16);
            }
            else if (player.bigMario == true)
            {
                frames = new Vector2(image.Width / 16, image.Height / 32);
            }
            //currentFrame = new Vector2(0, 0);
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        public override void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            base.LoadContent(Content, image, text, position);
            frameCounter = 0;
            switchFrame = 100;
            //currentFrame = new Vector2(0, 0);
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameCounter >= switchFrame)
                {
                    frameCounter = 0;
                    currentFrame.X++;

                    if (currentFrame.X * FrameWidth >= image.Width)
                        currentFrame.X = 0;
                }
            }
            else
            {
                frameCounter = 0;
            }

            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}
