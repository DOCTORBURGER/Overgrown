using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Overgrown.Scenes;
using System;

namespace Overgrown.UI
{
    public class MenuEntry
    {
        public string Text { get; set; }

        public Vector2 Position { get; set; }

        public event EventHandler Selected;

        protected internal virtual void OnSelectEntry()
        {
            Selected?.Invoke(this, new EventArgs());
        }

        public MenuEntry(string Text)
        {
            this.Text = Text;
        }

        public virtual void Draw(MenuScene screen, bool isSelected, GameTime gameTime)
        {
            var color = isSelected ? Color.Yellow : Color.White;

            // Draw text, centered on the middle of each line.
            var screenManager = screen.SceneManager;
            var spriteBatch = screenManager.SpriteBatch;
            var font = screenManager.Font;

            var origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, Text, Position, color, 0,
                origin, 1.0f, SpriteEffects.None, 0);
        }

        public int GetHeight(MenuScene screen)
        {
            return (int)screen.SceneManager.Font.MeasureString(Text).Y;
        }

        public int GetWidth(MenuScene screen)
        {
            return (int)screen.SceneManager.Font.MeasureString(Text).X;
        }
    }
}
