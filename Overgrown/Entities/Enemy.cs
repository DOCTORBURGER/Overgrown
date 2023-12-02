using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Collisions;

namespace Overgrown.Entities
{
    public class Enemy
    {
        private Texture2D _texture;

        private BoundingRectangle _bounds;

        private Vector2 _position;

        public BoundingRectangle Bounds { get { return _bounds; } }

        public Vector2 Position { get { return _position; } }

        public bool Collected { get; set; } = false;

        public Enemy(Vector2 position)
        {
            _position = position;
            this._bounds = new BoundingRectangle(position - new Vector2(-16, -16), 32, 32);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/UglyEnemyTemp");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Collected) return;

            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
