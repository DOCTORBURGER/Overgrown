using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Collisions;

namespace Overgrown.Entities
{
    public class Enemy
    {
        private Texture2D _texture;
        private Texture2D _textureHitbox;

        private const int SPRITE_HEIGHT = 32;
        private const int SPRITE_WIDTH = 32;

        private BoundingRectangle _bounds;

        private Vector2 _position;

        public BoundingRectangle Bounds { get { return _bounds; } }

        public Vector2 Position { get { return _position; } set { _position = value; } }

        public bool Collected { get; set; } = false;

        public bool DrawFinally { get; set; } = false;

        public Enemy(Vector2 position)
        {
            _position = position;
            this._bounds = new BoundingRectangle(position - new Vector2(16, 16), 32, 32);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/UglyEnemyTemp");
            _textureHitbox = content.Load<Texture2D>("Sprites/buttonbad");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Collected && !DrawFinally) return;

            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2), 1, SpriteEffects.None, 0);

            //Rectangle debugBoxRectangle = new Rectangle(0, 0, (int)Bounds.Width, (int)Bounds.Height);
            //spriteBatch.Draw(_textureHitbox, new Vector2(_bounds.X, _bounds.Y), debugBoxRectangle, new Color(100, 100, 100, 100));
        }
    }
}
