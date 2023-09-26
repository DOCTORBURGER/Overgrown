using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Overgrown.Entities
{
    public class Cloud
    {
        private Vector2 position = new Vector2(0, 0);

        private Texture2D texture;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("cloud");
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            position += new Vector2(1, 0) * 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (position.X > graphicsDevice.Viewport.Width) position.X = 0 - (int)(texture.Width * 0.3);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), 0.30f, SpriteEffects.None, 0);
        }
    }
}
