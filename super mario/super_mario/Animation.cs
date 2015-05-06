using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class Animation
    {
        protected Texture2D image;
        protected string text;
        protected SpriteFont font;
        protected Color color;
        protected Rectangle sourceRect;
        protected float rotation, scale, axis, alpha;
        protected Vector2 origin, position;
        protected ContentManager content;
        protected bool isActive;

        public Texture2D Image
        {
            get { return image; }
        }
        public Rectangle SourceRect
        {
            set { sourceRect = value; }
        }

        public virtual float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public float Scale
        {
            set { scale = value; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public virtual void LoadContent(ContentManager Content, Texture2D texture, string text, Vector2 position)
        {
            this.content = new ContentManager(Content.ServiceProvider, "Content");
            this.image = texture;
            this.text = text;
            this.position = position;
            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("Fonts/Font1");
                color = new Color(114, 77, 255);
            }

            

            if (texture != null)
            {
                sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            }

            alpha = 1f;
            rotation = 0.0f;
            axis = 0.0f;
            scale = 1.0f;
            isActive = false;
        }

        public virtual void LoadContent(ContentManager Content, Texture2D texture, string text, Vector2 position, Player player)
        {
            this.content = new ContentManager(Content.ServiceProvider, "Content");
            this.image = texture;
            this.text = text;
            this.position = position;
            if (text != String.Empty)
            {
                
            }
            font = this.content.Load<SpriteFont>("Fonts/Font1");
            color = new Color(114, 77, 255);


            if (texture != null)
            {
                sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            }

            alpha = 1f;
            rotation = 0.0f;
            axis = 0.0f;
            scale = 1.0f;
            isActive = false;
        }

        public virtual void UnloadContent()
        {
            content.Unload();
            text = String.Empty;
            position = Vector2.Zero;
            sourceRect = Rectangle.Empty;
            image = null;
        }

        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch, Effect aAffect = null)
        {
            if (image != null)
            {
                if (aAffect != null && Player.Instance.starMan == true)
                {
                    spriteBatch.End();
                    SamplerState sampler = SamplerState.PointClamp;
                    
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,sampler, null, null, aAffect, Camera.Instance.ViewMatrix);
                    //aAffect.CurrentTechnique.Passes[0].Apply();
                    spriteBatch.Draw(image, position + origin, sourceRect, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Instance.ViewMatrix);

                }
                else
                {
                    origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                    spriteBatch.Draw(image, position + origin, sourceRect, Color.White * alpha, rotation, origin, scale, SpriteEffects.None, 0f);
                }
            }

            if (text != String.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, position + origin, Color.White * alpha, rotation, origin, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
