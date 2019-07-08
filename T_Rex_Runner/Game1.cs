using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using T_Rex_Runner.Components;

namespace T_Rex_Runner
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static Random rnd = new Random();

        List<GameEntity> components = new List<GameEntity>();
        List<SoundEffect> soundEffects = new List<SoundEffect>();
        SoundEffectInstance soundInstance;
        SpriteLabel lblScore, lblGameOver;
        int highScore = 0;
        MenuItem btnReplay;
        bool isStart = false, isGameRunning = true, isDrawReplay = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //using (StreamReader sr = new StreamReader("Score.txt"))
            //{
            //    string s = sr.ReadLine();
            //    if (s != "")
            //        highScore = Int32.Parse(s);
            //}

            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            soundEffects.Add(Content.Load<SoundEffect>(@"Audio/Background_Song"));
            soundEffects.Add(Content.Load<SoundEffect>(@"Audio/Jump"));
            soundEffects.Add(Content.Load<SoundEffect>(@"Audio/Achieve"));
            soundEffects.Add(Content.Load<SoundEffect>(@"Audio/Die"));
            soundEffects.Add(Content.Load<SoundEffect>(@"Audio/Slide"));
            soundInstance = soundEffects[0].CreateInstance();
            soundInstance.IsLooped = true;

            components.Add(CreateNinja(new Vector2(50, 335)));
            components.Add(CreateLayer1(new Vector2(0, 0)));

            float widthMap = components[1].Model.Position.X + components[1].Model.FrameSize[0].X;
            components.Add(CreateLayer2(new Vector2(0, 0)));
            components.Add(CreateLayer2(new Vector2(widthMap, 0)));
            components.Add(CreateLayer2(new Vector2(widthMap * 2, 0)));

            components.Add(CreateLayer3(new Vector2(0, 0)));
            components.Add(CreateLayer3(new Vector2(widthMap, 0)));
            components.Add(CreateLayer3(new Vector2(widthMap * 2, 0)));

            components.Add(CreateLayer4(new Vector2(0, 0)));
            components.Add(CreateLayer4(new Vector2(widthMap, 0)));
            components.Add(CreateLayer4(new Vector2(widthMap * 2, 0)));

            components.Add(CreateLayer5(new Vector2(0, 0)));
            components.Add(CreateLayer5(new Vector2(widthMap, 0)));
            components.Add(CreateLayer5(new Vector2(widthMap * 2, 0)));

            lblScore = new SpriteLabel(Content.Load<SpriteFont>(@"Fonts/Score"), "", new Vector2(GraphicsDevice.Viewport.Width - 150, 20), Color.Black, 1);
            lblGameOver = new SpriteLabel(Content.Load<SpriteFont>(@"Fonts/Score"), "Game Over!", new Vector2(GraphicsDevice.Viewport.Width / 2 - 80, GraphicsDevice.Viewport.Height / 2 - 40), Color.Red, 1);
            btnReplay = CreateBtnReplay(new Vector2(GraphicsDevice.Viewport.Width / 2 - 30, GraphicsDevice.Viewport.Height / 2));
            btnReplay.Click += (object sender, EventArgs e) =>
            {
                RestartGame();
            };
        }
        private void RestartGame()
        {
            isGameRunning = true;
            isStart = false;
            isDrawReplay = false;
            // Reset game to initial state
            if (components.Count > 14)
                for (int i = 14; i < components.Count; i++)
                {
                    components[i].UnloadContent();
                    components.Remove(components[i]);
                }
            components[0].Model.Reset();

            float widthMap = components[1].Model.Position.X + components[1].Model.FrameSize[0].X;
            int numberOfDivSpeed = 0;
            if (currentScore >= 100)
                numberOfDivSpeed = 3;
            else if (currentScore >= 50)
                numberOfDivSpeed = 2;
            else if (currentScore >= 10)
                numberOfDivSpeed = 1;

            for (int i = 2; i < 14; i += 3)
            {
                ResetSpeed(numberOfDivSpeed, i);
                components[i].Model.SetPosition(0, 0);
            }
            for (int i = 3; i < 14; i += 3)
            {
                ResetSpeed(numberOfDivSpeed, i);
                components[i].Model.SetPosition(widthMap, 0);
            }
            for (int i = 4; i < 14; i += 3)
            {
                ResetSpeed(numberOfDivSpeed, i);
                components[i].Model.SetPosition(widthMap * 2, 0);
            }
            currentScore = 0;
        }
        private void ResetSpeed(int numberDivSpeed, int index)
        {
            for (int i = 1; i <= numberDivSpeed; i++)
                components[index].Model.Speed /= 2;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            for (int i = 0; i < components.Count; i++)
            {
                components[i].UnloadContent();
            }
        }

        private void IncreaseSpeedFollowLevel()
        {
            for (int i = 0; i < components.Count; i++)
                components[i].Model.IncreaseSpeed();
        }

        Matrix matrix = Matrix.Identity;
        int timeToCreateEnemy = rnd.Next(500, 1000);
        int timeSinceLastEnemy = 0;
        int speed = 10;
        int currentScore = 0;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //matrix = Matrix.CreateTranslation(x, y, 0f) * Matrix.CreateRotationZ(t) * Matrix.CreateScale(scale);

            components[0].Update(gameTime);
            lblScore.Text = "Score: " + currentScore;
            btnReplay.Update(gameTime);

            // When character die, we can restart game by Space or Up button
            if ((Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up))
                && components[0].Model.State == GLOBAL.SpriteState.Dead)
            {
                soundEffects[1].Play();
                RestartGame();
            }

            if (isGameRunning)
            {
                // Update all components
                if (components[0].Model.State == GLOBAL.SpriteState.Run)
                    isStart = true;

                if (isStart)
                {
                    soundInstance.Play();

                    timeSinceLastEnemy += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeSinceLastEnemy > timeToCreateEnemy)
                    {
                        timeSinceLastEnemy = 0;
                        timeToCreateEnemy = rnd.Next(500, 2000);
                        int i = rnd.Next(0, 4);
                        switch (i)
                        {
                            case 0:
                                components.Add(CreatMace(GetRandomPositionEnemy()));
                                break;
                            case 1:
                                components.Add(CreatMaceChain(GetRandomPositionEnemy()));
                                break;
                            case 2:
                                components.Add(CreatSaw(GetRandomPositionEnemy()));
                                break;
                            case 3:
                                components.Add(CreatSpike());
                                break;
                            default:
                                break;
                        }
                    }

                    for (int i = 1; i < components.Count; i++)
                    {
                        if (components[i] is Enemy &&
                                components[i].Model.Position.X + components[i].Model.FrameSize[0].X - components[i].Model.Speed < 0)
                        {
                            components[i].UnloadContent();
                            components.Remove(components[i]);
                            
                            currentScore += 1;
                            if (currentScore == 10 || currentScore == 50 || currentScore == 100)
                            {
                                soundEffects[2].Play();
                                IncreaseSpeedFollowLevel();
                            }
                        }
                        else
                            components[i].Update(gameTime);

                        //    //        if (components[j] is Angel)
                        //    //        {
                        //    //            Angel other = components[j] as Angel;
                        //    //            (components[i] as Angel).IsIntersect(other);
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    if (components[0].Model.State != GLOBAL.SpriteState.Slide && components[0].Model.State != GLOBAL.SpriteState.Jump)
                        soundEffects[1].Play();
                    components[0].Model.ChangeState(GLOBAL.SpriteState.Jump);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    if (components[0].Model.State != GLOBAL.SpriteState.Slide && components[0].Model.State != GLOBAL.SpriteState.Jump)
                        soundEffects[4].Play();
                    components[0].Model.ChangeState(GLOBAL.SpriteState.Slide);
                }

                if (components.Count > 14)
                for (int i = 14; i < components.Count; i++)
                {
                    if (components[0].Model.IsIntersect(components[i].Model))
                    {
                            soundInstance.Pause();
                            soundEffects[3].Play();
                        components[0].Model.ChangeState(GLOBAL.SpriteState.Dead);
                        if (currentScore > highScore)
                            {
                                highScore = currentScore;
                                //using (StreamWriter sr = new StreamWriter("Score.txt"))
                                //{
                                //    sr.Write(highScore);
                                //}
                            }
                        isGameRunning = false;
                            isDrawReplay = true;
                        return;
                    }
                }
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, matrix);
            // TODO: Add your drawing code here
            lblScore.Draw(gameTime, spriteBatch);

            if (isDrawReplay)
            {
                btnReplay.Draw(gameTime, spriteBatch);
                lblGameOver.Draw(gameTime, spriteBatch);
            }

            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is GameVisibleEntity)
                {
                    ((GameVisibleEntity)components[i]).Draw(gameTime, spriteBatch);

                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        Vector2 GetRandomPos(int y1, int y2)
        {
            return new Vector2(GraphicsDevice.Viewport.Width + 10, rnd.Next(y1, y2));
        }
        Vector2 GetRandomPositionEnemy()
        {
            int i = rnd.Next(2);
            if (i == 0)
                return new Vector2(GraphicsDevice.Viewport.Width + 10, 350);
            else return new Vector2(GraphicsDevice.Viewport.Width + 10, 280);
        }
        private MenuItem CreateBtnReplay(Vector2 position)
        {
            var tex = Content.Load<Texture2D>(@"Menu/Replay");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 0, 1, 0);
            return new MenuItem(this, sprite);
        }
        private GameVisibleEntity CreateLayer1(Vector2 position)
        {
            var tex = Content.Load<Texture2D>(@"Map/layer-1");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 0, 0, 0);
            return new Map(this, sprite);
        }
        private GameVisibleEntity CreateLayer2(Vector2 position)
        {
            var tex = Content.Load<Texture2D>(@"Map/layer-2");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 0, 0.9f, speed);
            return new Map(this, sprite);
        }
        private GameVisibleEntity CreateLayer3(Vector2 position)
        {
            var tex = Content.Load<Texture2D>(@"Map/layer-3");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 0, 0.8f, speed / 2);
            return new Map(this, sprite);
        }
        private GameVisibleEntity CreateLayer4(Vector2 position)
        {
            var tex = Content.Load<Texture2D>(@"Map/layer-4");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 0, 0.7f, speed / 5);
            return new Map(this, sprite);
        }
        private GameVisibleEntity CreateLayer5(Vector2 position)
        {
            var tex = Content.Load<Texture2D>(@"Map/layer-5");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 0, 0.6f, speed / 10);
            return new Map(this, sprite);
        }

        private GameVisibleEntity CreatMace(Vector2 position)
        {
            var tex = Content.Load<Texture2D>(@"Enemies/Mace");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 10, 0.9f);
            return new Enemy(this, sprite);
        }
        private GameVisibleEntity CreatSaw(Vector2 position)
        {
            var tex = Content.Load<Texture2D>(@"Enemies/Saw");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 10, 0.9f);
            return new Enemy(this, sprite);
        }
        private GameVisibleEntity CreatSpike()
        {
            var tex = Content.Load<Texture2D>(@"Enemies/Spike");
            var sprite = new Sprite2D(tex, new Vector2(GraphicsDevice.Viewport.Width + 10, 368), new Point(1, 1), 20, 0.9f);
            return new Enemy(this, sprite);
        }
        private GameVisibleEntity CreatMaceChain(Vector2 position)
        {
            var tex = Content.Load<Texture2D>(@"Enemies/Mace_Chain");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 10, 0.9f);
            return new Enemy(this, sprite);
        }

        private GameVisibleEntity CreateDinosaur(Vector2 position)
        {
            List<Texture2D> list = new List<Texture2D>();
            List<Point> sheetSize = new List<Point>();
            List<int> colideOffset = new List<int>();
            // Add sprite here
            list.Add(Content.Load<Texture2D>(@"Dinosaur/Dinosaur_Idle"));
            list.Add(Content.Load<Texture2D>(@"Dinosaur/Dinosaur_Run"));
            list.Add(Content.Load<Texture2D>(@"Dinosaur/Dinosaur_Jump"));
            list.Add(Content.Load<Texture2D>(@"Dinosaur/Dinosaur_Dead"));

            sheetSize.Add(new Point(10, 1));
            sheetSize.Add(new Point(8, 1));
            sheetSize.Add(new Point(6, 2));
            sheetSize.Add(new Point(8, 1));

            colideOffset.Add(10);
            colideOffset.Add(10);
            colideOffset.Add(20);
            colideOffset.Add(20);

            var sprite = new Sprite2D(list, position, sheetSize, colideOffset, 1);
            return new Dinosaur(this, sprite);

            //npc.Collide += (object sender, EventIntersectArgs e) => {
            //    this.Window.Title = string.Format("They collided to each other {0} - {1}", e.First.Model.Left, e.Second.Model.Left);
            //};
        }

        private GameVisibleEntity CreateNinja(Vector2 position)
        {
            List<Texture2D> list = new List<Texture2D>();
            List<Point> sheetSize = new List<Point>();
            List<int> colideOffset = new List<int>();
            // Add sprite here
            list.Add(Content.Load<Texture2D>(@"Ninja/Idle"));
            list.Add(Content.Load<Texture2D>(@"Ninja/Run"));
            list.Add(Content.Load<Texture2D>(@"Ninja/Jump"));
            list.Add(Content.Load<Texture2D>(@"Ninja/Slide"));
            list.Add(Content.Load<Texture2D>(@"Ninja/Dead"));

            sheetSize.Add(new Point(10, 1));
            sheetSize.Add(new Point(10, 1));
            sheetSize.Add(new Point(10, 1));
            sheetSize.Add(new Point(10, 1));
            sheetSize.Add(new Point(10, 1));

            colideOffset.Add(10);
            colideOffset.Add(10);
            colideOffset.Add(20);
            colideOffset.Add(10);
            colideOffset.Add(10);

            var sprite = new Sprite2D(list, position, sheetSize, colideOffset, 1);
            //npc.Collide += (object sender, EventIntersectArgs e) => {
            //    this.Window.Title = string.Format("They collided to each other {0} - {1}", e.First.Model.Left, e.Second.Model.Left);
            //};
            return new Ninja(this, sprite);
        }
    }
}
