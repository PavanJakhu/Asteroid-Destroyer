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
    public class MC : SpriteAnimationManager
    {
        public MC(ContentManager content, GraphicsDevice device)
            : base(content.Load<Texture2D>("Images/Megaman/Idle/MegamanIdle00"), new Vector2(100, 10), Vector2.Zero, true, 0.0f, 1.5f, SpriteEffects.None, content.Load<SoundEffect>("Sounds/Shoot"))
        {
            //Sets each animation to their own key.
            Animation a = new Animation();
            for (int i = 0; i < 6; i++)
                a.AddCell(content.Load<Texture2D>(StringUtilities.NextImageName("Images/Megaman/Idle/MegamanIdle00", i)));
            AddAnimation("Idle", a);

            a = new Animation();
            for (int i = 0; i < 10; i++)
                a.AddCell(content.Load<Texture2D>(StringUtilities.NextImageName("Images/Megaman/Run/MegamanRun00", i)));
            AddAnimation("Run", a);

            a = new Animation();
            for (int i = 0; i < 10; i++)
                a.AddCell(content.Load<Texture2D>(StringUtilities.NextImageName("Images/Megaman/Jump/MegamanJump00", i)));
            AddAnimation("Jump", a);

            a = new Animation();
            for (int i = 0; i < 4; i++)
                a.AddCell(content.Load<Texture2D>(StringUtilities.NextImageName("Images/Megaman/Shoot/MegamanShoot00", i)));
            AddAnimation("Shoot", a);

            CurrentAnimation = "Idle";
            animationDictionary["Idle"].LoopAll(1.5f);
        }

        /// <summary>
        /// Play idle animation and stop moving.
        /// </summary>
        public void Idle()
        {
            CurrentAnimation = "Idle";
            animationDictionary["Idle"].LoopAll(1.5f);
            base.Idle();
        }

        /// <summary>
        /// Play the run animation and move left.
        /// </summary>
        public void Left()
        {
            CurrentAnimation = "Run";
            animationDictionary["Run"].LoopAll(1.5f);
            base.Left();
        }

        /// <summary>
        /// Play run animation and move right. 
        /// </summary>
        public void Right()
        {
            CurrentAnimation = "Run";
            animationDictionary["Run"].LoopAll(1.5f);
            base.Right();
        }

        /// <summary>
        /// Play jump animation and move up.
        /// </summary>
        public void Jump()
        {
            CurrentAnimation = "Jump";
            animationDictionary["Jump"].PlayAll(2.0f);
            base.Jump();
        }

        /// <summary>
        /// Play shooting animation.
        /// </summary>
        public void Shoot()
        {
            CurrentAnimation = "Shoot";
            animationDictionary["Shoot"].PlayAll(1.0f);
        }
    }
}
