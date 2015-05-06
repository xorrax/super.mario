using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace super_mario
{
    public class MenuManager
    {
        List<string> menuItems, animationTypes, linkType, linkID;
        List<Texture2D> menuImages;
        List<Animation> tempAnimation;
        List<List<Animation>> animation;
        List<List<string>> attributes, contents;

        ContentManager content;
        FileManager fileManager;
        FadeAnimation fadeAnimation;
        Vector2 position;
        Rectangle source;
        SpriteFont font;

        int axis, itemNumber;
        string align;

        #region Private Methods

        private void SetMenuItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (menuImages.Count == i)
                    menuImages.Add(ScreenManager.Instance.NullImage);
            }
            for (int i = 0; i < menuImages.Count; i++)
            {
                if (menuItems.Count == i)
                    menuItems.Add("");
            }
        }

        private void SetAnimations()
        {
            Vector2 dimensions = Vector2.Zero;
            Vector2 addPosition = Vector2.Zero;

            if (align.Contains("Center"))
            {
                for (int i = 0; i < menuItems.Count; i++)
                {
                    dimensions.X += font.MeasureString(menuItems[i]).X + menuImages[i].Width;
                    dimensions.Y += font.MeasureString(menuItems[i]).Y + menuImages[i].Height;
                }

                if (axis == 1)
                {
                    addPosition.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;
                }
                else if (axis == 2)
                {
                    addPosition.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                }
            }
            else
            {
                addPosition = position;
            }

            tempAnimation = new List<Animation>();

            for (int i = 0; i < menuImages.Count; i++)
            {
                dimensions = new Vector2(font.MeasureString(menuItems[i]).X - menuImages[i].Width, font.MeasureString(menuItems[i]).Y - menuImages[i].Height);

                if (axis == 1)
                    addPosition.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                else
                    addPosition.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;

                for (int o = 0; o < animationTypes.Count; o++)
                {
                    switch (animationTypes[o])
                    {
                        case "Fade":
                            tempAnimation.Add(new FadeAnimation());
                            tempAnimation[tempAnimation.Count - 1].LoadContent(content, menuImages[i], menuItems[i], addPosition);
                            tempAnimation[tempAnimation.Count - 1].Font = font;
                            break;

                    }
                }
                if (tempAnimation.Count > 0)
                    animation.Add(tempAnimation);

                tempAnimation = new List<Animation>();


                if (axis == 1)
                {
                    addPosition.X += dimensions.X;
                }
                else
                {
                    addPosition.Y += dimensions.Y;
                }
            }
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            if (axis == 1)
            {
                if (inputManager.KeyPressed(Keys.D, Keys.Right))
                    itemNumber++;
                else if (inputManager.KeyPressed(Keys.A, Keys.Left))
                    itemNumber--;

            }
            else
            {
                if (inputManager.KeyPressed(Keys.S, Keys.Down))
                    itemNumber++;
                else if (inputManager.KeyPressed(Keys.W, Keys.Up))
                    itemNumber--;
            }

            if (inputManager.KeyPressed(Keys.Enter, Keys.X))
            {
                if (linkType[itemNumber] == "Screen")
                {
                    Type newClass = Type.GetType("super_mario." + linkID[itemNumber]);
                    ScreenManager.Instance.AddScreen((GameScreen)Activator.CreateInstance(newClass), inputManager);
                }
                else if (linkType[itemNumber] == "Exit")
                {
                    Game1.instance.Exit();
                }
            }

            if (itemNumber < 0)
                itemNumber = 0;
            else if (itemNumber > menuItems.Count - 1)
                itemNumber = menuItems.Count - 1;

            for (int i = 0; i < animation.Count; i++)
            {
                for (int o = 0; o < animation[i].Count; o++)
                {
                    if (itemNumber == i)
                        animation[i][o].IsActive = true;
                    else
                        animation[i][o].IsActive = false;

                    animation[i][o].Update(gameTime);
                }
            }
        }

        #endregion

        public void LoadContent(ContentManager content, string id)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            menuItems = new List<string>();
            animationTypes = new List<string>();
            linkType = new List<string>();
            linkID = new List<string>();
            menuImages = new List<Texture2D>();
            animation = new List<List<Animation>>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            itemNumber = 0;
            fadeAnimation = new FadeAnimation();

            position = Vector2.Zero;

            fileManager = new FileManager();
            if (Player.Instance.InCastle == true && Player.Instance.flagUp == true || Player.Instance.end == true)
            {
                Player.Instance.end = false;
                fileManager.LoadContent("Load/End.cme", attributes, contents, id);
            }
            else
                fileManager.LoadContent("Load/Menu.cme", attributes, contents, id);

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Font":
                            font = this.content.Load<SpriteFont>("Fonts/font1");
                            break;
                        case "Item":
                            menuItems.Add(contents[i][j]);
                            break;
                        case "Image":
                            menuImages.Add(this.content.Load<Texture2D>(contents[i][j]));
                            break;
                        case "Axis":
                            axis = int.Parse(contents[i][j]);
                            break;
                        case "Position":
                            string[] temp = contents[i][j].Split(' ');
                            position = new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));
                            break;
                        case "Source":
                            temp = contents[i][j].Split(' ');
                            source = new Rectangle(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]), int.Parse(temp[3]));
                            break;
                        case "Animation":
                            animationTypes.Add(contents[i][j]);
                            break;
                        case "Align":
                            align = contents[i][j];
                            break;
                        case "LinkType":
                            linkType.Add(contents[i][j]);
                            break;
                        case "LinkID":
                            linkID.Add(contents[i][j]);
                            break;
                    }
                }
            }
            SetMenuItems();
            SetAnimations();
        }

        public void UnloadContent()
        {
            content.Unload();
            fileManager = null;
            position = Vector2.Zero;
            menuItems.Clear();
            animation.Clear();
            menuImages.Clear();
            animationTypes.Clear();

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < animation.Count; i++)
            {
                for (int o = 0; o < animation[i].Count; o++)
                {
                    animation[i][o].Position = new Vector2(animation[i][o].Position.X + Camera.Instance.Position.X, animation[i][o].Position.Y);
                    animation[i][o].Draw(spriteBatch);
                }
            }
        }
    }
}
