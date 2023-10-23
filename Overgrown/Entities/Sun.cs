using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overgrown.Entities
{
    public class Sun
    {
        private Vector2 position = new Vector2(650, -25);

        private Texture2D texture;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sun");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), 5f, SpriteEffects.None, 0);
        }
    }
}
