using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

    public class PlayerSprite
    {
        private const float ANIMATION_SPEED = 0.05f;

        private KeyboardState keyboardState;

        private Texture2D texture;

        private bool flipped = false;

        private Vector2 position = new Vector2(200, 200);

        private PlayerState state = PlayerState.Idle;

        private int gravity = 3;

        private int animationFrame = 0;

        private double animationTimer;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("player");
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            state = PlayerState.Idle;

            if (keyboardState.IsKeyDown(Keys.A))
            {
                flipped = true; 
                position += new Vector2(-2, 0);
                state = PlayerState.Walking;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                flipped = false; 
                position += new Vector2(2, 0);
                state = PlayerState.Walking;
            }
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
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, new Vector2(64, 64), 1.25f, spriteEffects, 0);
        }
    }
}
