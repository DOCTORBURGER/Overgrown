using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overgrown.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overgrown.Entities
{
    public enum PlayerState
    {
        Idle,
        Running
    }

    public class Player
    {
        private const float ANIMATION_SPEED = 0.125f;

        private const float GRAVITY = 1500f;

        private const int SPRITE_HEIGHT = 64;
        private const int SPRITE_WIDTH = 64;

        private const int HITBOX_HEIGHT = 64;
        private const int HITBOX_WIDTH = 32;

        private KeyboardState keyboardState;
        private KeyboardState priorKeyboardState;

        private Texture2D texture;
        private Texture2D textureHitbox;

        private bool flipped = false;

        private Vector2 position = new Vector2(200, 180);

        private Vector2 velocity = new Vector2(0, 0);

        private PlayerState state = PlayerState.Idle;
        private PlayerState previousState = PlayerState.Idle;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - (HITBOX_WIDTH / 2), 180 - (HITBOX_HEIGHT / 2)), HITBOX_HEIGHT, HITBOX_WIDTH);

        private int animationFrame = 0;

        private double animationTimer;

        private SoundEffect jumpSound;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("player");
            textureHitbox = content.Load<Texture2D>("buttonbad");
            jumpSound = content.Load<SoundEffect>("jump");
        }

        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            priorKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            previousState = state;

            velocity.Y += t * GRAVITY;

            if (keyboardState.IsKeyDown(Keys.D) && !keyboardState.IsKeyDown(Keys.A))
            {
                flipped = false;
                velocity.X = 180;
                state = PlayerState.Running;
            }
            else if (keyboardState.IsKeyDown(Keys.A) && !keyboardState.IsKeyDown(Keys.D))
            {
                flipped = true;
                velocity.X = -180;
                state = PlayerState.Running;
            }
            else
            {
                velocity.X = 0;

                state = PlayerState.Idle;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && priorKeyboardState.IsKeyUp(Keys.Space))
            {
                velocity.Y = -600;
                jumpSound.Play();
            }

            position += velocity * t;

            if (position.X < 0 + (HITBOX_WIDTH / 2)) position.X = 0 + (HITBOX_WIDTH / 2);
            if (position.X > 800 - (HITBOX_WIDTH / 2)) position.X = 800 - (HITBOX_WIDTH / 2);
            if (position.Y < 0 + (HITBOX_HEIGHT / 2)) { position.Y = 0 + (HITBOX_HEIGHT / 2); velocity.Y = 0; }
            if (position.Y > 480 - (HITBOX_HEIGHT / 2)) { position.Y = 480 - (HITBOX_HEIGHT / 2); velocity.Y = 0; }

            bounds.X = position.X - (HITBOX_WIDTH / 2);
            bounds.Y = position.Y - (HITBOX_HEIGHT / 2);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (state != previousState)
            {
                animationFrame = 0;
            }

            if (animationTimer > ANIMATION_SPEED)
            {
                if (state == PlayerState.Idle)
                {
                    animationFrame++;
                    if (animationFrame > 9) animationFrame = 0;
                }
                if (state == PlayerState.Running)
                {
                    animationFrame++;
                    if (animationFrame > 7) animationFrame = 0;
                }
                animationTimer -= ANIMATION_SPEED;
            }

            Vector2 drawPosition = new Vector2((int)position.X, (int)position.Y);
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle sourceRectangle = new Rectangle(animationFrame * SPRITE_WIDTH, (int)state * SPRITE_HEIGHT, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Rectangle debugBoxRectangle = new Rectangle(0, 0, HITBOX_WIDTH, HITBOX_HEIGHT);
            spriteBatch.Draw(texture, drawPosition, sourceRectangle, Color.White, 0f, new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2), 1, spriteEffects, 0);
            //spriteBatch.Draw(textureHitbox, new Vector2(bounds.X, bounds.Y), debugBoxRectangle, new Color(100, 100, 100, 100));
        }
    }
}
