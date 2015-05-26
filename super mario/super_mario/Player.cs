using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace super_mario
{
    public class Player : Entity
    {
        SpriteFont scoreFont;
        float jumpSpeed = 0.2f;
        public float tileTimer = 0;
        bool walkingRight, walkingLeft, lastTransition, flagA, flagC, prevML, flagSetFrame, inCastle, diePlayed, setFrame, hit;
        public bool castle, flagDown, end, scPlayed, flagUp, bigMario, transition, jumpingRight, jumpingLeft, starMan, flagB, onFlag, tileCol, setPos;
        float transitionTimer = 0f;
        float transitionValue = 0f;
        float flagT;
        Matrix world; 
        int invertValue = 0;
        float starTimer = 0;
        float timer = 0;

        public int score = 0;

        static Player instance;

        float invertTimer = 0f;

        Queue<Vector2> flagWP = new Queue<Vector2>();

        //Sound
        SoundEffect smallJump, bigJump, powerup, gameOver, dieSound, stageClear;


        Effect myEffect;

        public bool InCastle
        {
            get { return inCastle; }
            set { inCastle = value; }
        }

        public bool Hit
        {
            get { return hit; }
            set { hit = value; }
        }

        public static Player Instance
        {
            get 
            {
                if (instance == null)
                    return new Player();

                return instance;
        
            }
        }

        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            fileManager = new FileManager();
            moveAnimation = new SpriteAnimation();
            Vector2 tempFrames = Vector2.Zero;
            moveSpeed = 100f;
            health = 1;
            smallJump = this.content.Load<SoundEffect>("Sound/smallJump");
            bigJump = this.content.Load<SoundEffect>("Sound/bigJump");
            powerup = this.content.Load<SoundEffect>("Sound/powerup");
            dieSound = this.content.Load<SoundEffect>("Sound/diesound");
            gameOver = this.content.Load<SoundEffect>("Sound/gameover");
            stageClear = this.content.Load<SoundEffect>("Sound/stageClear");
            scoreFont = this.content.Load<SpriteFont>("Fonts/scoreFont");
            scPlayed = false;
            flagDown = false;
            starMan = false;
            flagT = 0;
            flagSetFrame = false;
            inCastle = false;
            flagUp = false;
            tileCol = false;
            onFlag = false;
            flagB = false;
            setPos = false;
            diePlayed = false;
            setFrame = false;
            end = false;
            hit = false;

            instance = this;


            fileManager.LoadContent("Load/Player.cme", attributes, contents);

            myEffect = content.Load<Effect>("InvertColor");

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Health":
                            health = int.Parse(contents[i][j]);
                            break;
                        case "Image":
                            image = this.content.Load<Texture2D>(contents[i][j]);
                            break;
                        case "Position":
                            string [] frames = contents[i][j].Split(' ');
                            position = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                    }
                }
            }

            gravity = 100f;
            velocity = Vector2.Zero;
            syncTilePosition = false;
            activateGravity = true;
            FlagMovement();
            moveAnimation.Frames = new Vector2(image.Width / 16, image.Height / 16);
            moveAnimation.LoadContent(content, image, "", position, this);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Layer layer)
        {
            if (position.Y > ScreenManager.Instance.Dimensions.Y)
                health = 0;

            if (health == 0)
            {
                end = true;
            }

            if (end == true)
                dieSound.Play();

            if(prevML != OptionsScreen.mlValue)
            {
                if(OptionsScreen.mlValue == false)
                {
                    if (bigMario == false)
                        image = this.content.Load<Texture2D>("Player/smallMario");
                    else if (bigMario == true)
                        image = this.content.Load<Texture2D>("Player/bigMario");

                    moveAnimation.LoadContent(content, image, "", position, this);
                }
                else if(OptionsScreen.mlValue == true)
                {
                    if(bigMario == false)
                        image = this.content.Load<Texture2D>("Player/smallLuigi");
                    else if(bigMario == true)
                        image = this.content.Load<Texture2D>("Player/bigLuigi");

                    moveAnimation.LoadContent(content, image, "", position, this);
                }
            }

            prevML = OptionsScreen.mlValue;

            instance = this;

            if (onFlag == false)
            {
                syncTilePosition = false;
                prevPos = position;

                //Starman
                if (starMan == true)
                {
                    invertTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    starTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                if (starTimer >= 10000)
                {
                    starMan = false;
                    starTimer = 0f;
                }

                //Movement + sprite
                moveAnimation.IsActive = true;

                if (input.KeyPressed(Keys.W, Keys.Up) && activateGravity == false)
                {
                    //float height = 16;

                    //if (layer.tiles[(int)(position.X / 16)][(int)((position.Y + height) / 16) + 1].state == Tile.State.Solid ||
                    //    layer.tiles[(int)((position.X + 16f) / 16)][(int)((position.Y + height) / 16) + 1].state == Tile.State.Solid ||
                    //    !activateGravity)
                    //{
                    //    if (walkingRight == true)
                    //        jumpingRight = true;
                    //    else if (walkingLeft == true)
                    //        jumpingLeft = true;

                    //    velocity.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds - 4;
                    //    activateGravity = true;

                    //    if (bigMario == false)
                    //        smallJump.Play();
                    //    else if (bigMario == true)
                    //        bigJump.Play();
                    //}
                    //else
                    //{
                    //    position.X = position.X
                    //}
                    if (walkingRight == true)
                        jumpingRight = true;
                    else if (walkingLeft == true)
                        jumpingLeft = true;

                    velocity.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds - 4;
                    activateGravity = true;

                    if (bigMario == false)
                        smallJump.Play();
                    else if (bigMario == true)
                        bigJump.Play();
                }

                if (input.KeyDown(Keys.Right, Keys.D))
                {
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                    if (position.X >= Camera.Instance.Position.X + ScreenManager.Instance.Dimensions.X / 2)
                    {
                        Camera.Instance.SetCameraPoint(new Vector2(position.X, ScreenManager.Instance.Dimensions.Y / 2));
                    }
                    velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    walkingRight = true;
                    walkingLeft = false;
                }
                else if (input.KeyDown(Keys.Left, Keys.A) && position.X > Camera.Instance.Position.X)
                {
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                    //Camera.Instance.SetCameraPoint(new Vector2(Camera.Instance.Position.X, ScreenManager.Instance.Dimensions.Y / 2));
                    velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    walkingLeft = true;
                    walkingRight = false;
                }
                else
                {
                    moveAnimation.IsActive = false;
                    velocity.X = 0;
                }



                if (jumpingLeft == true && walkingLeft == true && walkingRight == false)
                {
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 3);
                }
                else if (jumpingRight == false && walkingRight == false)
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);


                if (jumpingRight == true && walkingRight == true && walkingLeft == false)
                {
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
                }
                else if (jumpingLeft == false && walkingLeft == false)
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);


                if (input.KeyReleased(Keys.Right, Keys.D))
                    moveAnimation.CurrentFrame = new Vector2(0, 0);
                else if (input.KeyReleased(Keys.Left, Keys.A))
                    moveAnimation.CurrentFrame = new Vector2(0, 1);

                if (activateGravity && transition == false)
                    velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds / 4;
                else
                {
                    velocity.Y = 0;
                }
                if (transition == false)
                    position += velocity;

                moveAnimation.Position = position;
                moveAnimation.Update(gameTime);
                //----------

                //Hit
                if (hit == true)
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (timer >= 1f)
                    {
                        hit = false;
                        timer = 0;
                    }
                }

                //Transition
                if (transition == true)
                {
                    if(starMan == false)
                        bigMario = !bigMario;

                    transitionTimer += (float)gameTime.ElapsedGameTime.Milliseconds;
                    if (lastTransition != transition)
                        powerup.Play();

                    if (transitionTimer >= 30f)
                    {
                        transitionValue += 1;
                        transitionTimer = 0;
                    }

                    if (transitionValue >= 10)
                    {
                        if (starMan == true)
                        {

                        }
                        else
                        {
                            if (OptionsScreen.mlValue == true)
                            {
                                if (bigMario == false)
                                {
                                    image = this.content.Load<Texture2D>("Player/smallLuigi");
                                }
                                else if (bigMario == true)
                                {
                                    image = this.content.Load<Texture2D>("Player/bigLuigi");
                                }
                            }
                            else if (OptionsScreen.mlValue == false)
                            {
                                if (bigMario == false)
                                {
                                    image = this.content.Load<Texture2D>("Player/smallMario");
                                }
                                else if (bigMario == true)
                                {
                                    image = this.content.Load<Texture2D>("Player/bigMario");
                                }
                            }
                            
                            moveAnimation.LoadContent(content, image, "", position, this);
                            
                            if (bigMario == false)
                                position = new Vector2(position.X, position.Y + 16);
                            else if (bigMario == true)
                                position = new Vector2(position.X, position.Y - 17);                                                     
                        }
                        
                        transition = false;
                        
                    }
                }

                if (health == 0f && diePlayed == false)
                {
                    diePlayed = true;
                    dieSound.Play();
                }



                lastTransition = transition;
                
            }
            //On flag-------------------
            else if (onFlag == true)
            {
                starMan = false;
                syncTilePosition = false;
                prevPos = position;

                if (scPlayed == false)
                {
                    stageClear.Play();
                    scPlayed = true;
                }

                if (setFrame == false)
                {
                    moveAnimation.CurrentFrame = new Vector2(0, 6);
                    setFrame = true;
                }

                if(flagB == true)
                {
                    if(flagT == 0)
                        moveAnimation.CurrentFrame = new Vector2(0, 7);
                    
                    flagT += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (flagT >= 100 && flagA == false && flagC == false)
                    {
                        flagA = true;
                    }
                }

                if(flagA == true)
                {
                    moveAnimation.CurrentFrame = new Vector2(0, 0);
                    flagC = true;
                    flagA = false;
                }

                if(flagWP.Count == 0)
                {
                    castle = true;
                }
                 //position == new Vector2(63 * 16, 13 * 16)   

                float speed = .5f;



                if (flagWP.Count > 2)
                {
                    
                    moveAnimation.Update(gameTime);
                    if (bigMario == true)
                    {
                        Vector2 bigPos = new Vector2(position.X, position.Y + 20);
                        Vector2 direction = flagWP.Peek() - bigPos;
                        direction.Normalize();
                        velocity = Vector2.Multiply(direction, speed);
                        position += velocity;
                        if (distanceToWP(bigPos) < speed)
                        {
                            position = flagWP.Peek();
                            flagWP.Dequeue();
                        }
                    }
                    else if (bigMario == false)
                    {
                        Vector2 direction = flagWP.Peek() - position;
                        //if(direction.X != 0 && direction.Y != 0)
                        direction.Normalize();
                        velocity = Vector2.Multiply(direction, speed);
                        position += velocity;
                        if (distanceToWP(position) < speed)
                        {
                            position = flagWP.Peek();
                            flagWP.Dequeue();
                        }
                    }
                }
                else if (flagWP.Count > 0 == ObjectHandler.instance.pF.flagDown == true)
                {
                    if (flagSetFrame == false)
                    {
                        moveAnimation.CurrentFrame = new Vector2(0, 0);
                        flagSetFrame = true;
                    }

                    if (bigMario == true)
                    {
                        Vector2 bigPos = new Vector2(position.X, position.Y + 16);
                        Vector2 direction = flagWP.Peek() - bigPos;
                        direction.Normalize();
                        velocity = Vector2.Multiply(direction, speed);
                        position += velocity;


                        moveAnimation.Update(gameTime);
                        if (distanceToWP(bigPos) < 1.0f)
                        {
                            position = flagWP.Peek();
                            flagWP.Dequeue();
                        }
                    }
                    else if (bigMario == false)
                    {
                        Vector2 direction = flagWP.Peek() - position;
                        direction.Normalize();
                        velocity = Vector2.Multiply(direction, speed);
                        position += velocity;
                        moveAnimation.Update(gameTime);
                        if (distanceToWP(position) < speed)
                        {
                            position = flagWP.Peek();
                            flagWP.Dequeue();
                        }
                    }
                }
                else if (flagWP.Count == 0)
                {
                    inCastle = true;
                    end = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (castle == false)
            {
                myEffect.Parameters["g_Texture"].SetValue(Image);
                myEffect.Parameters["View"].SetValue(Camera.Instance.ViewMatrix);
                world = Matrix.CreateTranslation(new Vector3(position, 0));
                myEffect.Parameters["World"].SetValue(world);
                myEffect.Parameters["Invert"].SetValue(invertValue);

                if (starMan == true)
                {
                    if (invertTimer >= 100 && invertValue == 0)
                    {
                        invertValue = 1;
                        invertTimer = 0f;
                    }
                    else if (invertTimer >= 100 && invertValue == 1)
                    {
                        invertValue = 0;
                        invertTimer = 0f;
                    }
                }
                else
                    invertValue = 0;
                if (inCastle == true)
                {
                }
                else if (transition == false)
                {
                    moveAnimation.Draw(spriteBatch, myEffect);
                }
                else if (transition == true && transitionTimer <= 10)
                {
                    moveAnimation.Draw(spriteBatch, myEffect);
                }
            }
            spriteBatch.DrawString(scoreFont, "Score: " + score.ToString(), new Vector2(Camera.Instance.Position.X
                + 16 * 11, 5), Color.White);
        }

        public void FlagMovement()
        {
            flagWP.Enqueue(new Vector2(68 * 16 - 5, 12*16));
            flagWP.Enqueue(new Vector2(70 * 16, 13 * 16));
            flagWP.Enqueue(new Vector2(73 * 16, 13 * 16));
        }

        public float distanceToWP(Vector2 pos)
        {
            return Vector2.Distance(pos, flagWP.Peek());
            //get { return Vector2.Distance(pos, flagWP.Peek()); }
        }
    }
}
