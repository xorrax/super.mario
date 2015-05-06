using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class FadeAnimation : Animation
    {
        bool increase, stopUpdating;
        float fadeSpeed, activateNumber, defaultAlpha;
        TimeSpan defaultTime, timer;

        public TimeSpan Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public float FadeSpeed
        {
            get { return fadeSpeed; }
            set { fadeSpeed = value; }
        }

        public override float Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = value;

                if (alpha == 1.0f)
                    increase = false;
                else if (alpha == 0.0f)
                    increase = true;
            }
        }

        public float ActivateNumber
        {
            get { return activateNumber; }
            set { activateNumber = value; }
        }

        public override void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            base.LoadContent(Content, image, text, position);
            increase = false;
            fadeSpeed = 1.0f;
            defaultTime = new TimeSpan(0, 0, 1);
            timer = defaultTime;
            activateNumber = 0.0f;
            stopUpdating = false;
            defaultAlpha = alpha;
        }

        public override void Update(GameTime gameTime)
        {
            MathHelper.Clamp(alpha, 0, 1);
            MathHelper.Clamp(activateNumber, 0, 1);
            if (isActive)
            {
                if (!stopUpdating)
                {

                    if (!increase)
                        alpha -= fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                        alpha += fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (alpha <= 0.0f)
                    {
                        alpha = 0.0f;
                        increase = true;
                    }
                    else if (alpha >= 1.0f)
                    {
                        alpha = 1.0f;
                        increase = false;
                    }
                }

                if (alpha == activateNumber)
                {
                    stopUpdating = true;
                    timer -= gameTime.ElapsedGameTime;
                    if (timer.TotalSeconds <= 0)
                    {
                        timer = defaultTime;
                        stopUpdating = false;
                    }
                }
            }
            else
            {
                alpha = defaultAlpha;
                stopUpdating = false;
            }
        }
    }
}
