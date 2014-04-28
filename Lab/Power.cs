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
    public enum Powers
    {
        Down, Up, None
    }

    public class Power : SpriteFromSheet
    {
        private Powers p;
        private bool colliding;

        public Power(ContentManager content, GraphicsDevice device, List<Sprite> asteroids, float powerSpeed)
            : base(content.Load<Texture2D>("Images/PowerDown"), new Vector2(device.Viewport.Width, Game1.ran.Next(device.Viewport.Height)), new Vector2(powerSpeed, 0), true, 0.0f, 0.5f, SpriteEffects.None, null, new Vector2(75, 75), Vector2.Zero, new Vector2(6, 8), 1.0f)
        {
            //If the power up or down sprite is colliding with an asteroid then randomize the position again.
            colliding = true;
            while (colliding)
            {
                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (asteroids[i].CollisionSprite(this))
                    {
                        colliding = true;
                        this.Position = new Vector2(device.Viewport.Width, Game1.ran.Next(device.Viewport.Height));
                    }
                    else
                        colliding = false;
                }
            }

            p = Powers.None;
            //Randomly decide if the power is an up or down.
            switch (Game1.ran.Next(2))
            {
                case (int)Powers.Down:
                    p = Powers.Down;
                    break;
                case (int)Powers.Up:
                    this.Image = content.Load<Texture2D>("Images/PowerUp");
                    this.SheetSize = new Vector2(6, 4);
                    p = Powers.Up;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Activates the effect of whatever the power up or down is.
        /// </summary>
        /// <param name="sprite">The sprite to make move faster.</param>
        /// <param name="bulletSpeed">The bullet speed.</param>
        /// <returns>The modified bullet speed if the power is a power up.</returns>
        public float Effect(Sprite sprite, float bulletSpeed)
        {
            if (p == Powers.Down)
            {
                sprite.Speed *= 1.5f;
                return bulletSpeed;
            }
            else if (p == Powers.Up)
                return bulletSpeed * 2;
            else
                return bulletSpeed;
        }
    }
}
