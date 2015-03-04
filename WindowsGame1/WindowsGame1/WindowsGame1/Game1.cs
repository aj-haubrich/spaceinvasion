using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue trackCue;
        SoundEffect soundEffect;
        protected Song song;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        MouseState prevMouseState;
        Texture2D allien;
        Point allienFrameSize = new Point(50, 44);
        Point allienCurrentFrame = new Point(22, 0);
        Point allienSheetSize = new Point(12, 1);
        int allienTimeSinceLastFrame = 0;
        int allienMillisecondsPerFrame = 90;
        Vector2 allienPosition = new Vector2(100, 100);
        Vector2 shipPosition = new Vector2(100, 100);
        const float shipSpeed = 6;
        public Random rnd { get; private set; }

        Texture2D texture;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 90;
        int shipCollisionRectOffset = 10;
        int allienCollisionRectOffset = 10;
        Point frameSize = new Point(100, 100);
        Point currentFrame = new Point(0, 0);
        Point sheetSize = new Point(9, 1);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rnd = new Random();
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            base.Initialize();
        }
     
        /*protected bool Collide( )
        {
            Rectangle alliensRect = new Rectangle(
            (int)allienPosition.X + allienCollisionRectOffset,
            (int)allienPosition.Y + allienCollisionRectOffset,
            allienFrameSize.X - (allienCollisionRectOffset * 2),
            allienFrameSize.Y - (allienCollisionRectOffset * 2));
            Rectangle shipRect = new Rectangle(
            (int)shipPosition.X + shipCollisionRectOffset,
            (int)shipPosition.Y + shipCollisionRectOffset,
            frameSize.X - (shipCollisionRectOffset * 2),
            frameSize.Y - (shipCollisionRectOffset * 2));
            return alliensRect.Intersects(shipRect);
        }*/
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            soundEffect = Content.Load<SoundEffect>(@"Audio\opentreasurechest");
            song = Content.Load<Song>(@"Audio\RondoVenezianoCattedrali");
        //   audioEngine = new AudioEngine(@"Content/Audio/GameAudio.xgs");
         //   waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
         //   soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            // Start the soundtrack audio
            // Start the soundtrack audio
          // Cue trackCue = soundBank.GetCue("horsebattlemusic");
            //trackCue.Play();
       
            // Play the start sound
           // soundBank.PlayCue("opentreasurechest");
            // Play the sound
            soundEffect.Play();
            MediaPlayer.Play(song); 
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
           // texture = Content.Load<Texture2D>(@"images\ship");
           // allien = Content.Load<Texture2D>(@"images\miniufo");
            // TODO: use this.Content to load your game content here
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
           /* // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }

            * 
            allienTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (allienTimeSinceLastFrame > allienMillisecondsPerFrame)
                {
                    allienTimeSinceLastFrame -= allienMillisecondsPerFrame;
                    ++allienCurrentFrame.X;
                    if (allienCurrentFrame.X >= allienSheetSize.X)
                    {
                    allienCurrentFrame.X = 0;
                       ++allienCurrentFrame.Y;
                    if (allienCurrentFrame.Y >= allienSheetSize.Y)
                        allienCurrentFrame.Y = 0;
                    }
                }

                   KeyboardState keyboardState = Keyboard.GetState( );
                        if (keyboardState.IsKeyDown(Keys.Left))
                            shipPosition.X -= shipSpeed;
                        if (keyboardState.IsKeyDown(Keys.Right))
                            shipPosition.X += shipSpeed;
                        if (keyboardState.IsKeyDown(Keys.Up))
                            shipPosition.Y -= shipSpeed;
                        if (keyboardState.IsKeyDown(Keys.Down))
                            shipPosition.Y += shipSpeed;


                        MouseState mouseState = Mouse.GetState();
                        if (mouseState.X != prevMouseState.X ||
                        mouseState.Y != prevMouseState.Y)
                            shipPosition = new Vector2(mouseState.X, mouseState.Y);
                        prevMouseState = mouseState;

                        if (shipPosition.X < 0)
                            shipPosition.X = 0;
                        if (shipPosition.Y < 0)
                            shipPosition.Y = 0;
                        if (shipPosition.X > Window.ClientBounds.Width - frameSize.X)
                            shipPosition.X = Window.ClientBounds.Width - frameSize.X;
                        if (shipPosition.Y > Window.ClientBounds.Height - frameSize.Y)
                            shipPosition.Y = Window.ClientBounds.Height - frameSize.Y;


                        if (Collide())
                            Exit();
            */
         //   audioEngine.Update();
                        base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
        /*    spriteBatch.Draw(texture, shipPosition,
            new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y,
            frameSize.X,
            frameSize.Y),
            Color.White, 0, Vector2.Zero,
            1, SpriteEffects.None, 0);*/

        /*    spriteBatch.Draw(allien, new Vector2(50,44),
                new Rectangle(allienCurrentFrame.X * allienFrameSize.X,
                    allienCurrentFrame.Y * allienFrameSize.Y,
                    allienFrameSize.X,
                    allienFrameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    1, SpriteEffects.None, 0); */
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
