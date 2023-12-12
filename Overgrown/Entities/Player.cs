using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overgrown.Collisions;
using Overgrown.State_Management;

namespace Overgrown.Entities
{
    public enum PlayerState
    {
        Idle,
        Running,
        Jumping
    }

    public class Player
    {
        private const float ANIMATION_SPEED = 0.125f;
        private const float GRAVITY = 1500f;
        private const int SPRITE_HEIGHT = 64;
        private const int SPRITE_WIDTH = 64;
        private const int HITBOX_HEIGHT = 64;
        private const int HITBOX_WIDTH = 32;
        private const float COYOTE_TIME = 0.25f;

        InputState _input = new InputState();

        private readonly InputAction _left = new InputAction(new[] { Keys.A }, false);
        private readonly InputAction _right = new InputAction(new[] { Keys.D }, false);
        private readonly InputAction _jump = new InputAction(new[] { Keys.Space }, true);

        private Texture2D _texture;
        private Texture2D _textureHitbox;

        private SoundEffect _jumpSound;

        private bool _flipped = false;
        private bool _grounded = false;
        private Vector2 _position;
        private Vector2 _velocity = new Vector2(0, 0);
        private BoundingRectangle _bounds;

        private PlayerState _state = PlayerState.Idle;
        private PlayerState _previousState = PlayerState.Idle;

        private int _animationFrame = 0;
        private double _animationTimer;

        private double _coyoteTimer;

        public Vector2 Position { get { return _position; } set { _position = value; } }

        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

        public bool Grounded { get { return _grounded; } set { _grounded = value; } }

        public BoundingRectangle Bounds { get { return _bounds; } set { } }

        public Player(Vector2 position)
        {
            _position = position;
            _bounds = new BoundingRectangle(_position, HITBOX_WIDTH, HITBOX_HEIGHT);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/player");
            _textureHitbox = content.Load<Texture2D>("Sprites/buttonbad");
            _jumpSound = content.Load<SoundEffect>("SoundEffects/jump");
        }

        public void Update(GameTime gameTime)
        {
            _input.Update();

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _previousState = _state;

            _velocity.Y += t * GRAVITY;

            _velocity.X = 0;
            _state = PlayerState.Idle;

            if (_grounded) { _coyoteTimer = 0; }

            if (_right.Occurred(_input) && !_left.Occurred(_input))
            {
                _flipped = false;
                _velocity.X = 250;
                _state = PlayerState.Running;
            }
            else if (_left.Occurred(_input) && !_right.Occurred(_input))
            {
                _flipped = true;
                _velocity.X = -250;
                _state = PlayerState.Running;
            }

            if (_jump.Occurred(_input) && (_grounded || _coyoteTimer < COYOTE_TIME))
            {
                _grounded = false;
                _velocity.Y = -750;
                _jumpSound.Play();
                _coyoteTimer = COYOTE_TIME;
                _state = PlayerState.Jumping;
                _animationFrame = 0;
            }

            if (_grounded == false  && _previousState == PlayerState.Jumping)
            {
                _coyoteTimer += t;
                _state = PlayerState.Jumping;
            }
            else if (_grounded == false && _state != PlayerState.Jumping)
            {
                _previousState = PlayerState.Jumping;
                _state = PlayerState.Jumping;
                _animationFrame = 3;
            }

            if (_state == PlayerState.Jumping && _velocity.Y < 0 && _jump.Released(_input))
            {
                _velocity.Y = 0;
            }

            _position += _velocity * t;

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
                _animationFrame++;
                if (_state == PlayerState.Idle)
                {
                    if (_animationFrame > 9) _animationFrame = 0;
                }
                if (_state == PlayerState.Running)
                {
                    if (_animationFrame > 7) _animationFrame = 0;
                }
                if (_state == PlayerState.Jumping)
                {
                    if (_animationFrame > 8) _animationFrame = 8;
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
            _bounds.X = (int)(_position.X - (HITBOX_WIDTH / 2));
            _bounds.Y = (int)(_position.Y - (HITBOX_HEIGHT / 2));
        }
    }
}
