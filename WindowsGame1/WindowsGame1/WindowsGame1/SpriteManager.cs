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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        UserControlledSprite player;
        int enemySpawnMinMilliseconds = 1000;
        int enemySpawnMaxMilliseconds = 2000;
        int enemyMinSpeed = 2;
        int enemyMaxSpeed = 6;
        int nextSpawnTime = 0;
        List<Sprite> spriteList = new List<Sprite>();
        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            ResetSpawnTime();
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
       public override void Update(GameTime gameTime)
       {
           nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
           if (nextSpawnTime < 0)
            {
                SpawnEnemy( );
            // Reset spawn timer
                ResetSpawnTime( );
             }
            // Update player
            player.Update(gameTime, Game.Window.ClientBounds);

        // Update all sprites
        foreach (Sprite s in spriteList)
        {
           s.Update(gameTime, Game.Window.ClientBounds);
          // Check for collisions and exit game if there is one
            if (s.collisionRect.Intersects(player.collisionRect))
                Game.Exit( );
        }
        // Update all sprites
        for (int i = 0; i < spriteList.Count; ++i)
        {
            Sprite s = spriteList[i];
            s.Update(gameTime, Game.Window.ClientBounds);
            // Check for collisions
            if (s.collisionRect.Intersects(player.collisionRect))
            {
                // Play collision sound
            //    if (s.collisionCueName != null)
            //        ((Game1)Game).PlayCue(s.collisionCueName);
                // Remove collided sprite from the game
                spriteList.RemoveAt(i);
                --i;
            }
            // Remove object if it is out of bounds
            if (s.IsOutOfBounds(Game.Window.ClientBounds))
            {
                spriteList.RemoveAt(i);
                --i;
            }
        }
        base.Update(gameTime);
       }
        public override void Draw(GameTime gameTime)
        {   
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            // Draw the player
            player.Draw(gameTime, spriteBatch);
            // Draw all sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/ship"),
            Vector2.Zero, new Point(100, 100), 10, new Point(0, 0), new Point(9, 1), new Vector2(9, 1));
            spriteList.Add(new AllienSprite(
            Game.Content.Load<Texture2D>(@"Images/miniufo"),
            new Vector2(400, 200), new Point(52, 45), 10, new Point(20, 0),
            new Point(9, 1), new Vector2(1,1), this));
            spriteList.Add(new AutomatedSprite(
            Game.Content.Load<Texture2D>(@"Images/asteroid1"),
            new Vector2(200, 100), new Point(72, 72), 10, new Point(0, 0),
            new Point(5, 2), new Vector2(1, 1)));
            spriteList.Add(new AutomatedSprite(
            Game.Content.Load<Texture2D>(@"Images/asteroid1"),
            new Vector2(300, 100), new Point(72, 72), 10, new Point(0, 0),
            new Point(5, 2), new Vector2(1, 1)));
            spriteList.Add(new AutomatedSprite(
            Game.Content.Load<Texture2D>(@"Images/asteroid1"),
            new Vector2(600, 400), new Point(72, 72), 10, new Point(0, 0),
            new Point(5, 2), new Vector2(1,1)));
            base.LoadContent();
        }

        private void ResetSpawnTime()
        {
            nextSpawnTime = ((Game1)Game).rnd.Next(
            enemySpawnMinMilliseconds,
            enemySpawnMaxMilliseconds);
        }

        private void SpawnEnemy( )
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;
            // Default frame size
            Point frameSize = new Point(75, 75);
            // Randomly choose which side of the screen to place enemy,
            // then randomly create a position along that side of the screen
            // and randomly choose a speed for the enemy
            switch (((Game1)Game).rnd.Next(4))
        {
        case 0: // LEFT to RIGHT
            position = new Vector2(-frameSize.X, ((Game1)Game).rnd.Next(0,
            Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));
            speed = new Vector2(((Game1)Game).rnd.Next(enemyMinSpeed,enemyMaxSpeed), 0);
        break;
        case 1: // RIGHT to LEFT
            position = new Vector2(Game.GraphicsDevice.PresentationParameters.BackBufferWidth,((Game1)Game).rnd.Next(0,
            Game.GraphicsDevice.PresentationParameters.BackBufferHeight - frameSize.Y));
            speed = new Vector2(-((Game1)Game).rnd.Next(
            enemyMinSpeed, enemyMaxSpeed), 0);
        break;
        case 2: // BOTTOM to TOP
            position = new Vector2(((Game1)Game).rnd.Next(0,
            Game.GraphicsDevice.PresentationParameters.BackBufferWidth - frameSize.X),
            Game.GraphicsDevice.PresentationParameters.BackBufferHeight);
            speed = new Vector2(0,-((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed));
        break;
        case 3: // TOP to BOTTOM
            position = new Vector2(((Game1)Game).rnd.Next(0,
            Game.GraphicsDevice.PresentationParameters.BackBufferWidth - frameSize.X), -frameSize.Y);
            speed = new Vector2(0, ((Game1)Game).rnd.Next(enemyMinSpeed, enemyMaxSpeed));
            break;
        }
        // Create the sprite
      //  spriteList.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\miniufo"),
        //    position, new Point(52, 45), 10, new Point(22, 0),
        //new Point(12, 1), speed));
        spriteList.Add(new AllienSprite(Game.Content.Load<Texture2D>(@"images\miniufo"),
        position, new Point(54, 45), 10, new Point(70, 0),
        new Point(12, 1), speed, this));
        }

        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }
    }

       
}
