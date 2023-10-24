using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Overgrown.Collisions;
using Microsoft.Xna.Framework.Audio;

namespace Overgrown.UI
{
    public class Button
    {
        #region Fields

        private string _text;

        private Vector2 _buttonPosition;

        private Vector2 _textPosition;

        private Texture2D _texture;

        private SpriteFont _font;

        private MouseState _currentMouse;

        private MouseState _previousMouse;

        private bool _mouseIsHovering;
        private bool _previousMouseIsHovering;

        private SoundEffect _selectBlipSound;

        #endregion

        #region Properties 

        public BoundingRectangle Rectangle
        {
            get
            {
                return new BoundingRectangle((int)_buttonPosition.X, (int)_buttonPosition.Y, _texture.Width, _texture.Height);
            }
        }

        private Color color = Color.White;

        public event EventHandler Click;

        #endregion

        public Button(string text, Vector2 buttonPosition)
        {
            this._text = text;
            this._buttonPosition = buttonPosition;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("buttonbad");
            _font = content.Load<SpriteFont>("button_font");

            _selectBlipSound = content.Load<SoundEffect>("blipSelect");
        }

        public void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new BoundingRectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _previousMouseIsHovering = _mouseIsHovering;
            _mouseIsHovering = false;

            color = Color.White;

            if (CollisionHelper.Collides(Rectangle, mouseRectangle))
            {
                _mouseIsHovering = true;
                color = Color.Gray;
                if (!_previousMouseIsHovering) _selectBlipSound.Play();

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var textSize = _font.MeasureString(_text);
            _textPosition = new Vector2(_buttonPosition.X + ((300 - textSize.X) / 2), _buttonPosition.Y + ((75 - textSize.Y) / 2));
            spriteBatch.Draw(_texture, _buttonPosition, color);
            spriteBatch.DrawString(_font, _text, _textPosition, Color.White);
        }
    }
}
