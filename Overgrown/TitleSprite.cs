using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SharpDX.Direct3D9;

namespace Overgrown
{
    public class TitleSprite
    {
        private const float ANIMATION_SPEED = 0.3f;

        private double animationTimer;

        private int animationFrame;

        private Vector2 position = new Vector2(0, 175);

        private Texture2D texture;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("title");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame >= 4) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            var source = new Rectangle(animationFrame * 800, 0, 800, 480);
            spriteBatch.Draw(texture, position, source, Color.White);
        }
    }
}
