using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Overgrown
{
    public class Button
    {
        #region Fields

        private string text;

        private Vector2 buttonPosition;

        private Vector2 textPosition;

        private Texture2D texture;

        private SpriteFont font;

        private MouseState currentMouse;

        private MouseState previousMouse;

        private bool mouseIsHovering;

        #endregion

        #region Properties 

        private Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, texture.Width, texture.Height);
            }
        }

        public event EventHandler Click;

        #endregion

        public Button(string text, Vector2 buttonPosition)
        {
            this.text = text;
            this.buttonPosition = buttonPosition;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("buttonbad");
            font = content.Load<SpriteFont>("button_font");
        }

        public void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            mouseIsHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                mouseIsHovering = true;

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var textSize = font.MeasureString(text);
            textPosition = new Vector2(buttonPosition.X + ((300 - textSize.X) / 2), buttonPosition.Y + ((75 - textSize.Y) / 2));
            spriteBatch.Draw(texture, buttonPosition, Color.White);
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }
    }
}
