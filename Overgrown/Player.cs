using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overgrown.Collisions;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overgrown
{
    public enum PlayerState
    {
        Idle,
        Walking
    }

    public class Player
    {
        private const float ANIMATION_SPEED = 0.05f;

        private const float GRAVITY = 1000f;

        private KeyboardState keyboardState;

        private Texture2D texture;
        private Texture2D textureHitbox;

        private bool flipped = false;

        private Vector2 position = new Vector2(200, 200);

        private Vector2 velocity = new Vector2(0, 0);

        private PlayerState state = PlayerState.Idle;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 25, 200 - 25), 32, 32);

        private float scale = 1.25f;

        private int animationFrame = 0;

        private double animationTimer;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("player");
            textureHitbox = content.Load<Texture2D>("buttonbad");
        }

        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            keyboardState = Keyboard.GetState();

            state = PlayerState.Idle;

            velocity.Y += t * GRAVITY;

            if (keyboardState.IsKeyDown(Keys.D) && !keyboardState.IsKeyDown(Keys.A))
            {
                flipped = false;
                velocity.X = 180;
                state = PlayerState.Walking;
            }
            else if (keyboardState.IsKeyDown(Keys.A) && !keyboardState.IsKeyDown(Keys.D))
            {
                flipped = true;
                velocity.X = -180;
                state = PlayerState.Walking;
            }
            else
            {
                velocity.X = 0;
            }
            
            position += velocity * t;

            if (position.X < 0 + 25) position.X = 0 + 25;
            if (position.X > 800 - 25) position.X = 800 - 25;
            if (position.Y < 0 + 25) position.Y = 0 + 25;
            if (position.Y > 480 - 25) position.Y = 480 - 25;

            bounds.X = position.X - 25;
            bounds.Y = position.Y - 25;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (state == PlayerState.Idle)
            {
                animationFrame = 0;
            }

            if (animationTimer > ANIMATION_SPEED)
            {
                if (state == PlayerState.Walking)
                {
                    animationFrame++;
                    if (animationFrame > 5) animationFrame = 0;
                    animationTimer -= ANIMATION_SPEED;
                }
            }

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle sourceRectangle = new Rectangle(animationFrame * 50, (int)state * 50, 50, 50);
            Rectangle debugBoxRectangle = new Rectangle(0, 0, 50, 50);
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, new Vector2(25, 25), 1, spriteEffects, 0);
            //spriteBatch.Draw(textureHitbox, new Vector2(bounds.X, bounds.Y), debugBoxRectangle, Color.White);
        }
    }
}
