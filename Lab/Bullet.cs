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
    public class Bullet : Sprite
    {
        public Bullet(ContentManager content, GraphicsDevice device, Sprite mainCharacter, float bulletSpeed)
            : base(content.Load<Texture2D>("Images/Bullet"), mainCharacter.Position + new Vector2(mainCharacter.Image.Width / 2 + 10, -13), new Vector2(bulletSpeed, 0), true, 0.0f, 1.0f, SpriteEffects.None, null)
        {
            if (mainCharacter.SpriteEffect == SpriteEffects.FlipHorizontally) //If the player is facing left instead of right.
            {
                this.Position = mainCharacter.Position - new Vector2(mainCharacter.Image.Width / 2 - 10, 13);
                this.Velocity = new Vector2(-bulletSpeed, 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            velocity.Y += 1; //Add gravity to the bullet (projectile motion).
            base.Update(gameTime);
        }
    }
}
