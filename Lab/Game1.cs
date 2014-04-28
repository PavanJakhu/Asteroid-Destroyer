#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using SpriteClasses;
#endregion

namespace Final
{
    enum Level { Top, Bottom }
    enum GameState { Main, How, InGame, Lose }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Random object for randomization.
        /// </summary>
        public static Random ran;

        /// <summary>
        /// The current game state the game is in.
        /// </summary>
        GameState gameState = GameState.Main;

        /// <summary>
        /// The previous mouse state.
        /// </summary>
        MouseState prevMouseState;
        /// <summary>
        /// Check the previous keyboard state.
        /// </summary>
        KeyboardState prevKeyState;
        /// <summary>
        /// Checks the current gamepad state.
        /// </summary>
        GamePadState prevPadState;

        /// <summary>
        /// The texture for the title screen.
        /// </summary>
        Texture2D title;
        /// <summary>
        /// The texture for the losing screen.
        /// </summary>
        Texture2D lose;
        /// <summary>
        /// The texture for the how to play screen.
        /// </summary>
        Texture2D how;

        /// <summary>
        /// The menu music.
        /// </summary>
        SoundEffect menuSong;
        /// <summary>
        /// The instance of the menu music so it can be looped.
        /// </summary>
        SoundEffectInstance menuBackSong;
        /// <summary>
        /// The in game music.
        /// </summary>
        SoundEffect inGameSong;
        /// <summary>
        /// The instance of the in game music so it can be looped.
        /// </summary>
        SoundEffectInstance inGameBackSong;
        /// <summary>
        /// The start sound effect.
        /// </summary>
        SoundEffect startSound;

        /// <summary>
        /// The moving space background.
        /// </summary>
        ParallaxBackground background;

        /// <summary>
        /// The main character.
        /// </summary>
        Sprite MCharacter;

        /// <summary>
        /// The list of power ups and downs on screen.
        /// </summary>
        List<Sprite> PowerEffects;

        /// <summary>
        /// The amount of time passed until a power up or down spawns.
        /// </summary>
        float timePassedSincePowerSpawn;
        /// <summary>
        /// The time at which the power up or down spawns.
        /// </summary>
        float spawnPower;
        /// <summary>
        /// Checks if the player already has a power up.
        /// </summary>
        bool powerOn;
        /// <summary>
        /// Checks if a power up in the list has been deleted.
        /// </summary>
        bool powerDeleted;
        /// <summary>
        /// The amount of time that has passed until the player loses the power up or down.
        /// </summary>
        float timePassedToReset;
        /// <summary>
        /// The time at which the power up or down is rest on the player.
        /// </summary>
        float resetPower;
        /// <summary>
        /// The amount the power up or down moves at.
        /// </summary>
        float powerSpeed;

        /// <summary>
        /// The container for bullets on screen.
        /// </summary>
        List<Sprite> Bullets;
        /// <summary>
        /// Checks if the player has fired a bullet.
        /// </summary>
        bool fired;
        /// <summary>
        /// The speed of the bullets.
        /// </summary>
        float bulletSpeed;

        /// <summary>
        /// The metal platform texture.
        /// </summary>
        Texture2D metalTex;

        /// <summary>
        /// The peices of metal.
        /// </summary>
        List<Sprite>[] Metal;
        /// <summary>
        /// The first platform.
        /// </summary>
        Sprite[] beginning;

        /// <summary>
        /// The time passed since the last platform has spawned.
        /// </summary>
        float timePassedSincePlatSpawn;
        /// <summary>
        /// The number of seconds that platforms spawn after the beginning one.
        /// </summary>
        float spawnPlat;
        /// <summary>
        /// How fast the platforms move.
        /// </summary>
        float platSpeed;

        /// <summary>
        /// The list of asteroids on screen.
        /// </summary>
        List<Sprite> Ast;

        /// <summary>
        /// The time passed since the last asteroid has spawned.
        /// </summary>
        float timePassedSinceAstSpawn;
        /// <summary>
        /// The number of seconds a asterioid spawns.
        /// </summary>
        float spawnAst;
        /// <summary>
        /// The speed of the asteroids.
        /// </summary>
        float astSpeed;

        /// <summary>
        /// The font style used for most HUD.
        /// </summary>
        SpriteFont verdana;
        /// <summary>
        /// The number seconds the player has survived.
        /// </summary>
        float timerSec;
        /// <summary>
        /// The number of minutes the player has survived.
        /// </summary>
        float timerMin;
        /// <summary>
        /// The string to hold the time.
        /// </summary>
        string timer;
        /// <summary>
        /// The position on the screen of where the timer will be displayed.
        /// </summary>
        Vector2 timerPos;

        /// <summary>
        /// The score of the player.
        /// </summary>
        float points;
        /// <summary>
        /// The score of the player to draw to the screen.
        /// </summary>
        string pointString;
        /// <summary>
        /// The position of the player's score on screen.
        /// </summary>
        Vector2 pointPos;

        /// <summary>
        /// The amount of time passed until the game becomes harder where things move faster and etc.
        /// </summary>
        float timePassedToIncreaseDifficulty;
        /// <summary>
        /// The time at which the difficulty of the game is raised.
        /// </summary>
        float timetoIncreaseDifficulty;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.PreferredBackBufferWidth = 469; //Window's width.
            //graphics.PreferredBackBufferHeight = 349; //Window's height.
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ran = new Random();

            Bullets = new List<Sprite>();
            fired = false;
            bulletSpeed = 700;

            beginning = new Sprite[10];
            Metal = new List<Sprite>[2];
            for (int i = 0; i < Metal.Length; i++)
                Metal[i] = new List<Sprite>();
            timePassedSincePlatSpawn = 0;
            spawnPlat = platSpeed = 5.0f;

            Ast = new List<Sprite>();
            astSpeed = -300;
            Ast.Add(new Asteroid(Content, GraphicsDevice, astSpeed));
            timePassedSinceAstSpawn = 0;
            spawnAst = 2.0f;

            PowerEffects = new List<Sprite>();
            powerSpeed = -200;
            PowerEffects.Add(new Power(Content, GraphicsDevice, Ast, powerSpeed));
            timePassedSincePowerSpawn = timePassedToReset = 0.0f;
            spawnPower = 2.0f;
            resetPower = 8.0f;
            powerOn = powerDeleted = false;

            timePassedToIncreaseDifficulty = 0;
            timetoIncreaseDifficulty = 5.0f;

            timer = "";
            timerSec = timerMin = 0;
            timerPos = new Vector2(GraphicsDevice.Viewport.Width / 1.1f, 30);

            points = 0;
            pointPos = new Vector2(GraphicsDevice.Viewport.Width - GraphicsDevice.Viewport.Width / 1.1f - 40, 30);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            try //To check if anything loads incorrectly.
            {
                MCharacter = new MC(Content, GraphicsDevice);
                background = new ParallaxBackground(GraphicsDevice);

                title = Content.Load<Texture2D>("Images/Title");
                lose = Content.Load<Texture2D>("Images/Lose");
                how = Content.Load<Texture2D>("Images/How");

                menuSong = Content.Load<SoundEffect>("Sounds/MainMusic");
                menuBackSong = menuSong.CreateInstance();
                menuBackSong.IsLooped = true;
                inGameSong = Content.Load<SoundEffect>("Sounds/InGameMusic");
                inGameBackSong = inGameSong.CreateInstance();
                inGameBackSong.IsLooped = true;

                startSound = Content.Load<SoundEffect>("Sounds/Start");

                background.AddLayer(Content.Load<Texture2D>("Images/Background"), 0, -100);
                background.AddLayer(Content.Load<Texture2D>("Images/AsteroidBack"), 1, -100);
                background.StartMoving();

                verdana = Content.Load<SpriteFont>("Fonts/Verdana");

                metalTex = Content.Load<Texture2D>("Images/metal");

                for (int i = 0; i < beginning.Length; i++)
                    beginning[i] = new Sprite(metalTex, new Vector2(i * metalTex.Width * 0.05f + 50, 400), new Vector2(5, 0), false, 0.0f, 0.05f, SpriteEffects.None, null);

                SpawnPlatforms();
            }
            catch (ContentLoadException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + " Please contact the programmers.", e.GetType().ToString());
                Exit();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Main:
                    KeyboardState currKeyState = Keyboard.GetState();
                    menuBackSong.Play();
                    if (currKeyState.IsKeyDown(Keys.Enter))
                    {
                        menuBackSong.Stop(); //Stops the background music in the main menu.
                        inGameBackSong.Play(); //Plays the in game music.
                        startSound.Play(); //A sound effect plays telling the game has started.
                        gameState = GameState.InGame; //Switching game states.
                    }
                    else if (currKeyState.IsKeyDown(Keys.H))
                        gameState = GameState.How;
                    break;
                case GameState.How:
                    KeyboardState KeyState = Keyboard.GetState();
                    if (KeyState.IsKeyDown(Keys.Escape))
                        gameState = GameState.Main;
                    break;
                case GameState.InGame:
                    //Update each part of the game.
                    UpdateInput();

                    UpdateTimer(gameTime);

                    UpdateDifficulty(gameTime);

                    background.Update(gameTime);

                    UpdatePlayer(gameTime);

                    UpdatePlatforms(gameTime);

                    UpdateBullets(gameTime);

                    UpdateAsteroids(gameTime);

                    UpdatePowerDownUps(gameTime);
                    break;
                case GameState.Lose:
                    KeyboardState currentKeyState = Keyboard.GetState();
                    if (currentKeyState.IsKeyDown(Keys.Enter))
                    {
                        Initialize(); //Reset everything to their default values.
                        gameState = GameState.InGame;
                    }
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the sprites depending on if the player pressed up, down, left or right. Also the main character follows the mouse.
        /// </summary>
        private void UpdateInput()
        {
            //Gets the gamepad and keyboard states every frame.
            KeyboardState currKeyState = Keyboard.GetState();
            GamePadState currPadState = GamePad.GetState(PlayerIndex.One);
            MouseState currMouseState = Mouse.GetState();

            if (prevKeyState.IsKeyDown(Keys.R) && currKeyState.IsKeyDown(Keys.R))
            {
                Initialize(); //Reset everything to their default values.
                gameState = GameState.InGame;
            }

            if (((MC)MCharacter).CurrentAnimation != "Jump" && ((MC)MCharacter).CurrentAnimation != "Shoot") //Do normal animations if the player isn't jumping or shooting.
            {
                if ((currKeyState.IsKeyUp(Keys.Left) || currKeyState.IsKeyUp(Keys.Right))
                    || (currKeyState.IsKeyUp(Keys.A) || currKeyState.IsKeyUp(Keys.D))
                    || currPadState.ThumbSticks.Left.X == 0.0f)
                    ((MC)MCharacter).Idle();

                if (currKeyState.IsKeyDown(Keys.Left) || currKeyState.IsKeyDown(Keys.A) || currPadState.ThumbSticks.Left.X < 0)
                    ((MC)MCharacter).Left();
                if (currKeyState.IsKeyDown(Keys.Right) || currKeyState.IsKeyDown(Keys.D) || currPadState.ThumbSticks.Left.X > 0)
                    ((MC)MCharacter).Right();
            }
            else if (((MC)MCharacter).CurrentAnimation == "Jump")
            {
                if ((currKeyState.IsKeyUp(Keys.Left) || currKeyState.IsKeyUp(Keys.Right))
                    || (currKeyState.IsKeyUp(Keys.A) || currKeyState.IsKeyUp(Keys.D))
                    || currPadState.ThumbSticks.Left.X == 0.0f)
                    MCharacter.Idle();

                //Allows for the player to move while he is jumping.
                if (currKeyState.IsKeyDown(Keys.Left) || currKeyState.IsKeyDown(Keys.A) || currPadState.ThumbSticks.Left.X < 0)
                    MCharacter.Left();
                if (currKeyState.IsKeyDown(Keys.Right) || currKeyState.IsKeyDown(Keys.D) || currPadState.ThumbSticks.Left.X > 0)
                    MCharacter.Right();

                if (((MC)MCharacter).animationDictionary["Jump"].CurrentCell == 9) //Switches back to idle animation after done jumping animation.
                {
                    ((MC)MCharacter).CurrentAnimation = "Idle";
                    ((MC)MCharacter).animationDictionary["Idle"].LoopAll(1.5f);
                }
            }
            else if (((MC)MCharacter).CurrentAnimation == "Shoot")
            {
                if ((currKeyState.IsKeyUp(Keys.Left) || currKeyState.IsKeyUp(Keys.Right))
                    || (currKeyState.IsKeyUp(Keys.A) || currKeyState.IsKeyUp(Keys.D))
                    || currPadState.ThumbSticks.Left.X == 0.0f)
                    MCharacter.Idle();

                //Allows for the player to move while he is shooting.
                if (currKeyState.IsKeyDown(Keys.Left) || currKeyState.IsKeyDown(Keys.A) || currPadState.ThumbSticks.Left.X < 0)
                    MCharacter.Left();
                if (currKeyState.IsKeyDown(Keys.Right) || currKeyState.IsKeyDown(Keys.D) || currPadState.ThumbSticks.Left.X > 0)
                    MCharacter.Right();

                if (!fired && ((MC)MCharacter).animationDictionary["Shoot"].CurrentCell == 1) //Fires the bullet on the frame it looks like he is shooting.
                {
                    MCharacter.Sound.Play();
                    Sprite bullet = new Bullet(Content, GraphicsDevice, MCharacter, bulletSpeed);
                    Bullets.Add(bullet);
                    fired = true;
                }

                if (((MC)MCharacter).animationDictionary["Shoot"].CurrentCell == 3) //Switches back to idle animation after done shooting animation.
                {
                    ((MC)MCharacter).CurrentAnimation = "Idle";
                    ((MC)MCharacter).animationDictionary["Idle"].LoopAll(1.5f);
                    fired = false;
                }
            }

            if ((currKeyState.IsKeyDown(Keys.Up) && prevKeyState.IsKeyUp(Keys.Up))
                || (currKeyState.IsKeyDown(Keys.W) && prevKeyState.IsKeyUp(Keys.W))
                || (prevPadState.Buttons.A == ButtonState.Released && currPadState.Buttons.A == ButtonState.Pressed))
                ((MC)MCharacter).Jump();

            if ((currKeyState.IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space))
                || (currPadState.Triggers.Right == 1.0f)) //Can only shoot once per 2 frames.
                ((MC)MCharacter).Shoot();

            prevKeyState = currKeyState;
            prevPadState = currPadState;
            prevMouseState = currMouseState;
        }

        /// <summary>
        /// Updates the timer and points every second.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        private void UpdateTimer(GameTime gameTime)
        {
            timerSec += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (timerSec > 1.0f)
                points++;
            if (timerSec > 60.0f)
            {
                timerMin++;
                timerSec = 0.0f;
            }
            if (timerSec < 10.0f)
                timer = "" + timerMin + ":0" + Math.Round(timerSec);
            else
                timer = "" + timerMin + ":" + Math.Round(timerSec);
        }

        /// <summary>
        /// Update the difficulty.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        private void UpdateDifficulty(GameTime gameTime)
        {
            if (beginning[beginning.Length - 1].CollisionRectangle.Right < 0) //Only update once the first platform is gone.
            {
                timePassedToIncreaseDifficulty += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                if (timePassedToIncreaseDifficulty > timetoIncreaseDifficulty)
                {
                    platSpeed += 0.1f;
                    astSpeed -= 1;
                    powerSpeed -= 2;
                    spawnAst -= 0.01f;
                    if (spawnAst <= 2.0f) spawnAst = 2.0f;
                    spawnPower += 0.1f;
                    spawnPlat -= 0.1f;
                    timePassedToIncreaseDifficulty = 0;
                }
            }
        }

        /// <summary>
        /// Update the player.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        private void UpdatePlayer(GameTime gameTime)
        {
            MCharacter.Update(gameTime, GraphicsDevice);

            for (int i = 0; i < Ast.Count; i++)
            {
                if (MCharacter.CollisionSprite(Ast[i]))
                {
                    timer = "Time: " + timer;
                    pointString = "Points: " + pointString;
                    gameState = GameState.Lose;
                    break;
                }
            }

            if (MCharacter.IsOffScreen(GraphicsDevice))
            {
                timer = "Time: " + timer;
                pointString = "Points: " + pointString;
                gameState = GameState.Lose;
            }
        }

        /// <summary>
        /// Update the platforms.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        private void UpdatePlatforms(GameTime gameTime)
        {
            if (beginning[beginning.Length - 1].CollisionRectangle.Right > 0) //Only update the platforms coming once the beginning platform is gone.
            {
                timePassedSincePlatSpawn += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (timePassedSincePlatSpawn > spawnPlat) //Spawn a platform every spawnPlay seconds.
                {
                    for (int i = 0; i < beginning.Length; i++)
                        beginning[i].Position -= beginning[i].Velocity;
                    //MCharacter.Position -= beginning[0].Velocity;
                }

                for (int i = 0; i < beginning.Length; i++)
                {
                    if (MCharacter.CollisionSprite(beginning[i]))
                    {
                        MCharacter.SetVelYZero();
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Metal.Length; i++)
                {
                    for (int j = 0; j < Metal[i].Count; j++)
                    {
                        Metal[i][j].Position -= Metal[i][j].Velocity;
                        if (MCharacter.CollisionSprite(Metal[i][j]))
                            MCharacter.SetVelYZero();
                        if (Metal[i][Metal[i].Count - 1].CollisionRectangle.Right < 0)
                            SpawnPlatforms();
                    }
                }
            }
        }

        /// <summary>
        /// Clear the platform that is off screen.
        /// Spawns the platforms at the edge of the screen.
        /// </summary>
        private void SpawnPlatforms()
        {
            int platLength = ran.Next(3, 7);
            switch (ran.Next(2))
            {
                case (int)Level.Top:
                    Metal[(int)Level.Top].Clear();
                    Metal[(int)Level.Bottom].Clear();
                    for (int i = 0; i < platLength; i++)
                        Metal[(int)Level.Top].Add(new Sprite(metalTex, new Vector2(i * metalTex.Width * 0.05f + GraphicsDevice.Viewport.Width - 2, 250), new Vector2(platSpeed, 0), false, 0.0f, 0.05f, SpriteEffects.None, null));
                    break;
                case (int)Level.Bottom:
                    Metal[(int)Level.Top].Clear();
                    Metal[(int)Level.Bottom].Clear();
                    for (int i = 0; i < platLength; i++)
                        Metal[(int)Level.Bottom].Add(new Sprite(metalTex, new Vector2(i * metalTex.Width * 0.05f + GraphicsDevice.Viewport.Width - 2, 400), new Vector2(platSpeed, 0), false, 0.0f, 0.05f, SpriteEffects.None, null));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Update the bullets fired.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        private void UpdateBullets(GameTime gameTime)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                bool deleteBullet = false;
                Bullets[i].Update(gameTime);
                for (int j = 0; j < Ast.Count; j++)
                {
                    if (Bullets[i].CollisionSprite(Ast[j]))
                    {
                        points += 100;
                        Ast.RemoveAt(j);
                        Bullets.RemoveAt(i);
                        deleteBullet = true;
                        break;
                    }
                }
                if (!deleteBullet && Bullets[i].IsOffScreen(GraphicsDevice)) //Don't delete the bullet if it hasn't been already deleted.
                    Bullets.RemoveAt(i);
            }
        }

        /// <summary>
        /// Update the asteroids.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        private void UpdateAsteroids(GameTime gameTime)
        {
            timePassedSinceAstSpawn += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if (timePassedSinceAstSpawn > spawnAst)
            {
                Ast.Add(new Asteroid(Content, GraphicsDevice, astSpeed)); //Spawn an asteroid.
                timePassedSinceAstSpawn = 0;
            }

            for (int i = 0; i < Ast.Count; i++)
            {
                Ast[i].Update(gameTime);
                if (Ast[i].Position.X + ((Asteroid)Ast[i]).FrameSize.X < 0)
                {
                    Ast.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Update the power ups or downs.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        private void UpdatePowerDownUps(GameTime gameTime)
        {
            timePassedSincePowerSpawn += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            timePassedToReset += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if (timePassedSincePowerSpawn > spawnPower)
            {
                PowerEffects.Add(new Power(Content, GraphicsDevice, Ast, powerSpeed));
                timePassedSincePowerSpawn = 0;
            }
            if (powerOn && timePassedToReset > resetPower) //Only reset if the player has a power up or down active.
            {
                powerOn = false;
                MCharacter.Speed = 500;
                bulletSpeed = 700;
                timePassedToReset = 0;
            }

            for (int i = 0; i < PowerEffects.Count; i++)
            {
                PowerEffects[i].Update(gameTime);
                powerDeleted = false;
                if (!powerOn && MCharacter.CollisionSprite(PowerEffects[i]))
                {
                    powerOn = true;
                    timePassedToReset = 0;
                    bulletSpeed = ((Power)PowerEffects[i]).Effect(MCharacter, bulletSpeed);
                    PowerEffects.RemoveAt(i);
                    powerDeleted = true;
                }
                if (!powerDeleted && PowerEffects[i].IsOffScreen(GraphicsDevice))
                    PowerEffects.RemoveAt(i);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (gameState)
            {
                case GameState.Main:
                    spriteBatch.Begin();
                    spriteBatch.Draw(title, Vector2.Zero, Color.White);
                    spriteBatch.End();
                    break;
                case GameState.How:
                    spriteBatch.Begin();
                    spriteBatch.Draw(how, Vector2.Zero, Color.White);
                    spriteBatch.End();
                    break;
                case GameState.InGame:
                    background.Draw();

                    spriteBatch.Begin();

                    if (beginning[beginning.Length - 1].CollisionRectangle.Right > 0)
                    {
                        for (int i = 0; i < beginning.Length; i++)
                            beginning[i].Draw(spriteBatch);
                    }
                    else
                    {
                        for (int i = 0; i < Metal.Length; i++)
                        {
                            for (int j = 0; j < Metal[i].Count; j++)
                                Metal[i][j].Draw(spriteBatch);
                        }
                    }

                    for (int i = 0; i < PowerEffects.Count; i++)
                        PowerEffects[i].Draw(spriteBatch);

                    for (int i = 0; i < Ast.Count; i++)
                        Ast[i].Draw(spriteBatch);

                    for (int i = 0; i < Bullets.Count; i++)
                        Bullets[i].Draw(spriteBatch);

                    MCharacter.Draw(spriteBatch);

                    spriteBatch.DrawString(verdana, timer, timerPos, Color.White);

                    pointString = points.ToString();
                    for (int i = pointString.Length; i < 8; i++) //Put a mask on the points.
                        pointString = "0" + pointString;
                    spriteBatch.DrawString(verdana, pointString, pointPos, Color.White);

                    spriteBatch.End();
                    break;
                case GameState.Lose:
                    spriteBatch.Begin();
                    spriteBatch.Draw(lose, Vector2.Zero, Color.White);
                    //Put the texts in the middle of the screen.
                    spriteBatch.DrawString(verdana, pointString, new Vector2(GraphicsDevice.Viewport.Width / 2 - (float)verdana.MeasureString(pointString).Length() / 2, 250), Color.White);
                    spriteBatch.DrawString(verdana, timer, new Vector2(GraphicsDevice.Viewport.Width / 2 - (float)verdana.MeasureString(timer).Length() / 2, 300), Color.White);
                    spriteBatch.End();
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
