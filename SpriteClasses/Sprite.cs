#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace SpriteClasses
{
    public class Sprite
    {
        protected Vector2 initialVelocity;
        /// <summary>
        /// The intial velocity of the sprite.
        /// </summary>
        public Vector2 InitialVelocity
        {
            get { return initialVelocity; }
            set { initialVelocity = value; }
        }

        protected Vector2 velocity;
        /// <summary>
        /// The current velocity of the sprite.
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        protected Vector2 origin;
        /// <summary>
        /// Where the orgin is on the sprite.
        /// </summary>
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        protected Vector2 position;
        /// <summary>
        /// The position vector of the sprite.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private float speed;
        /// <summary>
        /// The speed at which the sprite can move.
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// The effects you can use on the sprite.
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        /// The texture/sprite of the object.
        /// </summary>
        public Texture2D Image { get; set; }

        /// <summary>
        /// The sound the sprite makes.
        /// </summary>
        public SoundEffect Sound { get; set; }

        /// <summary>
        /// The collision rectangle around the sprite.
        /// </summary>
        public virtual Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - origin.X * Scale), (int)(position.Y - origin.Y * Scale), (int)(Image.Width * Scale), (int)(Image.Height * Scale));
            }
        }

        /// <summary>
        /// Checks if the sprite is alive or dead.
        /// </summary>
        public bool Alive { get; set; }

        /// <summary>
        /// Checks if the sprite wants the origin to be calculated to the middle or use the default (top-left).
        /// </summary>
        public bool UseOrigin { get; set; }

        /// <summary>
        /// Rotates the sprite around the origin.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// How many rotations per frame there are going to be.
        /// </summary>
        public float RotationSpeed { get; set; }

        /// <summary>
        /// The size of the sprite.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Initializes everything to their default values when creating object.
        /// </summary>
        /// <param name="textureImage">The sprite image.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="velocity">The velocity of the sprite on screen.</param>
        /// <param name="useOrigin">Use the middle of the sprite as the origin.</param>
        /// <param name="rotationSpeed">How fast the sprite is rotating around its origin.</param>
        /// <param name="scale">The size of the sprite.</param>
        /// <param name="spriteEffect">The effects you want to have on the sprite.</param>
        public Sprite(Texture2D textureImage, Vector2 position, Vector2 velocity, bool useOrigin, float rotationSpeed, float scale, SpriteEffects spriteEffect, SoundEffect sound)
        {
            Image = textureImage;
            this.position = position;
            this.initialVelocity = velocity;
            this.velocity = velocity;
            UseOrigin = useOrigin;
            if (UseOrigin)
                this.origin = new Vector2(Image.Width / 2.0f, Image.Height / 2.0f);
            RotationSpeed = rotationSpeed;
            Scale = scale;
            SpriteEffect = spriteEffect;
            Sound = sound;
            Alive = true;
            Speed = 500;
        }

        /// <summary>
        /// Draws the sprites if it is alive according to what is initialized 
        /// </summary>
        /// <param name="spriteBatch">The draw settings.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Alive) spriteBatch.Draw(Image, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffect, 0.0f);
        }

        /// <summary>
        /// Update the sprite every frame if it is alive according to the frame rate.
        /// </summary>
        /// <param name="gameTime">The game timing.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (Alive)
            {
                float elapsedtime = gameTime.ElapsedGameTime.Milliseconds / 1000f;
                Rotation += RotationSpeed * elapsedtime;
                Rotation = Rotation % (MathHelper.Pi * 2.0f);

                Position += Velocity * elapsedtime;
            }
        }

        /// <summary>
        /// Updates the game if it is alive and allows the sprite to bounced off the sides of the viewport.
        /// </summary>
        /// <param name="gameTime">The game timing.</param>
        /// <param name="Device">Renderer.</param>
        public virtual void Update(GameTime gameTime, GraphicsDevice Device)
        {
            if (Alive)
            {
                Update(gameTime);

                velocity.Y += 15; //Add gravity to the player.

                //if (Position.Y < Device.Viewport.Height - origin.Y * Scale)
                //    velocity.Y += 15;
                //else
                //    velocity.Y = 0;

                position.X = MathHelper.Clamp(Position.X, 0 + Origin.X * Scale, Device.Viewport.Width - origin.X * Scale);
                //position.Y = MathHelper.Clamp(Position.Y, 0 + Origin.Y * Scale, Device.Viewport.Height - origin.Y * Scale);
            }
        }

        /// <summary>
        /// Checks to see if the mouse collided with the rectangle around the sprite.
        /// </summary>
        /// <param name="x">The mouse's current x position.</param>
        /// <param name="y">The mouse's current y position.</param>
        /// <returns>True if they are colliding.</returns>
        public bool CollisionMouse(int x, int y)
        {
            return CollisionRectangle.Contains(x, y);
        }

        /// <summary>
        /// Checks to see if the sprite collided with another sprite.
        /// </summary>
        /// <param name="sprite">The other sprite that may collide.</param>
        /// <returns>True if they are colliding.</returns>
        public bool CollisionSprite(Sprite sprite)
        {
            return this.CollisionRectangle.Intersects(sprite.CollisionRectangle);
        }

        /// <summary>
        /// Check if the sprite is off screen.
        /// </summary>
        /// <param name="Device">The graphics device.</param>
        /// <returns>Returns true if the sprite is off screen.</returns>
        public virtual bool IsOffScreen(GraphicsDevice Device)
        {
            if (Position.X - Image.Width * Scale > Device.Viewport.Width || 
                Position.X + Image.Width * Scale < 0 || 
                Position.Y - Image.Height * Scale > Device.Viewport.Height || 
                Position.Y + Image.Height * Scale < 0)
                return true;
            return false;
        }

        /// <summary>
        /// Sets the velocity vector to 0.
        /// </summary>
        public virtual void Idle()
        {
            velocity.X = 0;
        }

        /// <summary>
        /// Moves the sprite to the left relative to its self.
        /// </summary>
        public virtual void Left()
        {
            SpriteEffect = SpriteEffects.FlipHorizontally;
            velocity.X -= Speed;
        }

        /// <summary>
        /// Moves the sprite to the right relative to its self.
        /// </summary>
        public virtual void Right()
        {
            SpriteEffect = SpriteEffects.None;
            velocity.X += Speed;
        }

        /// <summary>
        /// Moves the sprite upwards.
        /// </summary>
        public virtual void Jump()
        {
            velocity.Y -= Speed;
        }

        /// <summary>
        /// Sets the y component of the velocity to 0.
        /// </summary>
        public void SetVelYZero()
        {
            velocity.Y = 0;
        }
    }
}
