using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class OptionsScreen : GameScreen
    {
        static public float masterVolume = 0.4f;
        static public bool mlValue = false;
        Vector2 indicatorPos = Vector2.Zero;
        Vector2 selectorPos = new Vector2(16 * 12, 1);
        Texture2D lowerImage, higherImage, volumeBackground, volumeIndicator, mlTexture, selectionTexture;
        SpriteFont volumeFont;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);

            //Volume textures
            lowerImage = this.content.Load<Texture2D>("Options/Volume/lowerImage");
            higherImage = this.content.Load<Texture2D>("Options/Volume/higherImage");
            volumeBackground = this.content.Load<Texture2D>("Options/Volume/volumeBr");
            volumeIndicator = this.content.Load<Texture2D>("Options/Volume/volumeInd");
            mlTexture = this.content.Load<Texture2D>("Options/mlTexture");
            selectionTexture = this.content.Load<Texture2D>("Options/selector");
            volumeFont = this.content.Load<SpriteFont>("Fonts/Font1");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            lowerImage = null;
            higherImage = null;
            volumeBackground = null;
            volumeIndicator = null;
            mlTexture = null;
            selectionTexture = null;
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();


            //Volume controls, increase and lower
            if(inputManager.KeyPressed(Keys.D, Keys.Right) && masterVolume < 1.0f && masterVolume <= 0.8f)
            {
                masterVolume += 0.2f;
            }
            else if (inputManager.KeyPressed(Keys.A, Keys.Left) && masterVolume >= 0.0f && masterVolume >= 0.2f)
            {
                masterVolume -= 0.2f;
            }

            if(inputManager.KeyPressed(Keys.Escape))
            {
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }

            if(inputManager.KeyPressed(Keys.O))
            {
                if (ScreenManager.Instance.savedScreen == null)
                    ScreenManager.Instance.AddScreen(new GameScreen(), inputManager);
                else
                    ScreenManager.Instance.AddScreenBack(inputManager);
            }


            indicatorPos = new Vector2(6 + 5* 16 + (masterVolume * 10 / 2) * 16 + Camera.Instance.Position.X, 6 * 16 + 6);

            //Mario or Luigi
            if(mlValue == false)
                selectorPos = new Vector2(16 * 12 + Camera.Instance.Position.X, 1);
            else if(mlValue == true)
                selectorPos = new Vector2(16 * 12 + Camera.Instance.Position.X, 17);

            if(inputManager.KeyPressed(Keys.W, Keys.Up) && mlValue == true)
            {
                selectorPos.Y -= 16;
                mlValue = false;
            }
            else if(inputManager.KeyPressed(Keys.S, Keys.Down) && mlValue == false)
            {
                selectorPos.Y += 16;
                mlValue = true;
            }



            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //Mario or Luigi
            spriteBatch.Draw(mlTexture, new Vector2(16 * 12 + Camera.Instance.Position.X , 1), Color.White);
            spriteBatch.Draw(selectionTexture, selectorPos, Color.White);

            //Volume
            spriteBatch.DrawString(volumeFont, "Volume", new Vector2(6 * 16 - 1 + Camera.Instance.Position.X, 4 * 16 - 1), Color.White);
            spriteBatch.Draw(volumeBackground, new Vector2(5 * 16 - 1 + Camera.Instance.Position.X, 6 * 16 - 1), Color.White);
            if(indicatorPos != Vector2.Zero)
                spriteBatch.Draw(volumeIndicator, indicatorPos, Color.White);

            spriteBatch.Draw(lowerImage, new Vector2(5 * 16 - 18 + Camera.Instance.Position.X, 6 * 16 + 8), Color.White);
            spriteBatch.Draw(higherImage, new Vector2(5 * 16 + 97 + Camera.Instance.Position.X, 6 * 16 + 8), Color.White);
        }
    }
}
