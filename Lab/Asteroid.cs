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
    public class Asteroid : SpriteFromSheet
    {
        public Asteroid(ContentManager content, GraphicsDevice device, float astSpeed)
            : base(content.Load<Texture2D>("Images/Asteroid"), new Vector2(device.Viewport.Width, Game1.ran.Next(device.Viewport.Height - 64)), new Vector2(astSpeed, 0), true, 0.5f, 1.5f, SpriteEffects.None, null, new Vector2(44, 44), Vector2.Zero, new Vector2(60, 1), 1.0f)
        { }

        public override void Update(GameTime gameTime)
        {
            velocity.Y += 1; //Add gravity to the asteroids.
            base.Update(gameTime);
        }
    }
}
