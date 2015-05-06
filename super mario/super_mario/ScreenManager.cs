using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class ScreenManager
    {
        #region Variables

        GameScreen currentScreen;
        GameScreen newScreen;
        GameScreen savedScreen;

        ContentManager content;

        private static ScreenManager instance;


        Stack<GameScreen> screenStack = new Stack<GameScreen>();

        



        Vector2 dimensions;

        bool transition, transitionBack, transitionKeep;

        FadeAnimation fade;

        Texture2D fadeTexture, nullImage;

        InputManager inputManager;

        #endregion

        #region Properties

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();
                return instance;
            }
        }

        public ContentManager Content
        {
            get { return content; }
        }


        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Texture2D NullImage
        {
            get { return nullImage; }
        }
        #endregion

        #region Main Methods


        //Keep old screen
        public void AddScreenKeep(GameScreen screen, InputManager inputManager)
        {
            fade.Position = Camera.Instance.Position;
            transitionKeep = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.Alpha = 0.0f;
            fade.ActivateNumber = 1.0f;
            this.inputManager = inputManager;
        }
        //Change to old screen
        public void AddScreenBack(InputManager inputManager)
        {
            if (savedScreen != null)
            {
                fade.Position = Camera.Instance.Position;
                transitionBack = true;
                fade.IsActive = true;
                fade.Alpha = 0.0f;
                fade.ActivateNumber = 1.0f;
                this.inputManager = inputManager;
            }
            else
                transition = true;
        }

        //Remove old screen
        public void AddScreen(GameScreen screen, InputManager inputManager)
        {
            fade.Position = Camera.Instance.Position;
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.Alpha = 0.0f;
            fade.ActivateNumber = 1.0f;
            this.inputManager = inputManager;
        }

        public void Initialize()
        {
            currentScreen = new SplashScreen();
            fade = new FadeAnimation();
            inputManager = new InputManager();
        }

        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content, inputManager);

            nullImage = this.content.Load<Texture2D>("null");
            fadeTexture = this.content.Load<Texture2D>("fade");
            fade.LoadContent(content, fadeTexture, "", Vector2.Zero);
            fade.Scale = dimensions.X;

        }
        public void Update(GameTime gameTime)
        {
            if (!transition && !transitionBack && !transitionKeep)
                currentScreen.Update(gameTime);
            else if (transition)
                Transition(gameTime);
            else if (transitionBack)
                TransitionBack(gameTime);
            else if (transitionKeep)
                TransitionKeep(gameTime);

            Camera.Instance.Update();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            
            if (transition)
            {
                fade.Draw(spriteBatch);
            }
            else if (transitionBack)
            {
                fade.Draw(spriteBatch);
            }
            else if(transitionKeep)
            {
                fade.Draw(spriteBatch);
            }
            
                
        }

        #endregion


        private void Transition(GameTime gameTime)       
        {
            fade.Update(gameTime);

            if (fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScreen);
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent(content, inputManager);
            }
            else if (fade.Alpha == 0.0f)
            {
                transition = false;
                fade.IsActive = false;
            }
        }

        private void TransitionKeep(GameTime gameTime)
        {
            fade.Update(gameTime);

            if (fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScreen);
                savedScreen = currentScreen;
                currentScreen = newScreen;
                currentScreen.LoadContent(content, inputManager);
            }
            else if (fade.Alpha == 0.0f)
            {
                transitionKeep = false;
                fade.IsActive = false;
            }
        }

        private void TransitionBack(GameTime gameTime)
        {
            fade.Update(gameTime);

            if (fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(savedScreen);
                currentScreen.UnloadContent();
                currentScreen = savedScreen;
                //savedScreen.UnloadContent();
                //currentScreen.LoadContent(content, inputManager);
            }
            else if (fade.Alpha == 0.0f)
            {
                transitionBack = false;
                fade.IsActive = false;
            }
        }
    }
}
