﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace WindowsGame1
{
    class AllienSprite : Sprite
    {
        // Save a reference to the sprite manager to
        // use to get the player position
        SpriteManager spriteManager;
        public AllienSprite(Texture2D textureImage, Vector2 position,
        Point frameSize, int collisionOffset, Point currentFrame,
        Point sheetSize, Vector2 speed, SpriteManager spriteManager)
        : base(textureImage, position, frameSize, collisionOffset,
        currentFrame, sheetSize, speed)
    {
        this.spriteManager = spriteManager;
    }      
      public AllienSprite(Texture2D textureImage, Vector2 position,
      Point frameSize, int collisionOffset, Point currentFrame,
      Point sheetSize, Vector2 speed, int millisecondsPerFrame,
      SpriteManager spriteManager)
        : base(textureImage, position, frameSize, collisionOffset,
        currentFrame, sheetSize, speed, millisecondsPerFrame)

          {
            this.spriteManager = spriteManager;
            }
      public override Vector2 direction
        {
            get { return speed; }
        }
       public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
        // First, move the sprite along its direction vector
            position += speed;
        // Use the player position to move the sprite closer in
        // the X and/or Y directions
            Vector2 player = spriteManager.GetPlayerPosition( );
        // If player is moving vertically, chase horizontally
        if (speed.X == 0)
        {
          if (player.X < position.X)
                position.X -= Math.Abs(speed.Y);
          else if (player.X > position.X)
              position.X += Math.Abs(speed.Y);
        }
        // If player is moving horizontally, chase vertically
        if (speed.Y == 0)
        {
            if (player.Y < position.Y)
                position.Y -= Math.Abs(speed.X);
            else if (player.Y > position.Y)
              position.Y += Math.Abs(speed.X);
        }
           base.Update(gameTime, clientBounds);
        }
    }
}
            