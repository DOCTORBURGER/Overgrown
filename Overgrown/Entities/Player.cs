using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overgrown.Collisions;

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

        private KeyboardState _keyboardState;
        private KeyboardState _priorKeyboardState;

        private Texture2D _texture;
        private Texture2D _textureHitbox;

        private bool _flipped = false;

        private Vector2 _position = new Vector2(200, 180);

        private Vector2 _velocity = new Vector2(0, 0);

        private PlayerState _state = PlayerState.Idle;
        private PlayerState _previousState = PlayerState.Idle;

        private BoundingRectangle _bounds = new BoundingRectangle(new Vector2(200 - (HITBOX_WIDTH / 2), 180 - (HITBOX_HEIGHT / 2)), HITBOX_WIDTH, HITBOX_HEIGHT);

        private int _animationFrame = 0;

        private double _animationTimer;

        private SoundEffect _jumpSound;

        public Vector2 Position { get { return _position; } set { _position = value; }  }

        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

        public BoundingRectangle Bounds { get { return _bounds; } set { } }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("player");
            _textureHitbox = content.Load<Texture2D>("buttonbad");
            _jumpSound = content.Load<SoundEffect>("jump");
        }

        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _priorKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _previousState = _state;

            _velocity.Y += t * GRAVITY;

            if (_keyboardState.IsKeyDown(Keys.D) && !_keyboardState.IsKeyDown(Keys.A))
            {
                _flipped = false;
                _velocity.X = 180;
                _state = PlayerState.Running;
            }
            else if (_keyboardState.IsKeyDown(Keys.A) && !_keyboardState.IsKeyDown(Keys.D))
            {
                _flipped = true;
                _velocity.X = -180;
                _state = PlayerState.Running;
            }
            else
            {
                _velocity.X = 0;

                _state = PlayerState.Idle;
            }

            if (_keyboardState.IsKeyDown(Keys.Space) && _priorKeyboardState.IsKeyUp(Keys.Space))
            {
                _velocity.Y = -600;
                _jumpSound.Play();
            }

            _position += _velocity * t;

            if (_position.X < 0 + (HITBOX_WIDTH / 2)) _position.X = 0 + (HITBOX_WIDTH / 2);
            if (_position.X > 1600 - (HITBOX_WIDTH / 2)) _position.X = 1600 - (HITBOX_WIDTH / 2);
            if (_position.Y < 0 + (HITBOX_HEIGHT / 2)) { _position.Y = 0 + (HITBOX_HEIGHT / 2); _velocity.Y = 0; }
            if (_position.Y > 640 - (HITBOX_HEIGHT / 2)) { _position.Y = 640 - (HITBOX_HEIGHT / 2); _velocity.Y = 0; }

            UpdateBounds();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_state != _previousState)
            {
                _animationFrame = 0;
            }

            if (_animationTimer > ANIMATION_SPEED)
            {
                if (_state == PlayerState.Idle)
                {
                    _animationFrame++;
                    if (_animationFrame > 9) _animationFrame = 0;
                }
                if (_state == PlayerState.Running)
                {
                    _animationFrame++;
                    if (_animationFrame > 7) _animationFrame = 0;
                }
                _animationTimer -= ANIMATION_SPEED;
            }

            Vector2 drawPosition = new Vector2((int)_position.X, (int)_position.Y);
            SpriteEffects spriteEffects = (_flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle sourceRectangle = new Rectangle(_animationFrame * SPRITE_WIDTH, (int)_state * SPRITE_HEIGHT, SPRITE_WIDTH, SPRITE_HEIGHT);
            //Rectangle debugBoxRectangle = new Rectangle(0, 0, HITBOX_WIDTH, HITBOX_HEIGHT);
            spriteBatch.Draw(_texture, drawPosition, sourceRectangle, Color.White, 0f, new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2), 1, spriteEffects, 0);
            //spriteBatch.Draw(_textureHitbox, new Vector2(_bounds.X, _bounds.Y), debugBoxRectangle, new Color(100, 100, 100, 100));
        }

        public void UpdateBounds()
        {
            _bounds.X = _position.X - (HITBOX_WIDTH / 2);
            _bounds.Y = _position.Y - (HITBOX_HEIGHT / 2);
        }
    }
}
