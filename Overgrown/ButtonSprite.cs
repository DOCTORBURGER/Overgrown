using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Overgrown
{
    public class ButtonSprite
    {
        private string text;

        private Vector2 buttonPosition;

        private Vector2 textPosition;

        private Texture2D texture;

        private SpriteFont font;

        public ButtonSprite(string text, Vector2 buttonPosition)
        {
            this.text = text;
            this.buttonPosition = buttonPosition;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("buttonbad");
            font = content.Load<SpriteFont>("button_font");
        }

        public void Update()
        {

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
