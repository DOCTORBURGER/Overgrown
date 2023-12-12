using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Collisions;

namespace Overgrown.Entities
{
    public class EnemyWife
    {
        private const float ANIMATION_SPEED = 0.125f;
        private const int SPRITE_HEIGHT = 32;
        private const int SPRITE_WIDTH = 32;

        private int _animationFrame = 0;
        private double _animationTimer;

        private Texture2D _texture;
        private Texture2D _textureHitbox;

        private BoundingRectangle _bounds;

        private Vector2 _position;

        public BoundingRectangle Bounds { get { return _bounds; } }

        public Vector2 Position { get { return _position; } }

        public bool Collected { get; set; } = false;

        public EnemyWife(Vector2 position)
        {
            _position = position;
            this._bounds = new BoundingRectangle(position - new Vector2(16, 16), 32, 32);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/enemywife");
            _textureHitbox = content.Load<Texture2D>("Sprites/buttonbad");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_animationTimer > ANIMATION_SPEED)
            {
                _animationFrame++;
                _animationTimer -= ANIMATION_SPEED;
                if (_animationFrame > 15) { _animationFrame = 0; }
            }

            Rectangle sourceRectangle = new Rectangle(_animationFrame * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            spriteBatch.Draw(_texture, _position, sourceRectangle, Color.White, 0f, new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2), 1, SpriteEffects.None, 0);

            //Rectangle debugBoxRectangle = new Rectangle(0, 0, (int)Bounds.Width, (int)Bounds.Height);
            //spriteBatch.Draw(_textureHitbox, new Vector2(_bounds.X, _bounds.Y), debugBoxRectangle, new Color(100, 100, 100, 100));
        }
    }
}
