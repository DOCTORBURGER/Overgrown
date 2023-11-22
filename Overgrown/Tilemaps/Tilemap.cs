using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Overgrown.Tilemaps
{
    /// <summary>
    /// This tilemap class features an OO approach
    /// </summary>
    public class Tilemap
    {
        public TilemapLayer[] Layers { get; init; }

        public int Width;

        public int Height;

        public virtual void Update(GameTime gameTime)
        {
            foreach (var layer in Layers)
            {
                layer.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var layer in Layers)
            {
                if (layer.Visible) layer.Draw(gameTime, spriteBatch);
            }
        }
    }

    public class TilemapLayer
    {
        public TilemapTile[] Tiles { get; init; }

        public float Opacity { get; set; }

        public bool Visible { get; set; }

        public void Update(GameTime gameTime)
        {
            foreach (var tile in Tiles)
            {
                tile.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            foreach (var tile in Tiles)
            {
                tile.Draw(gameTime, spriteBatch);
            }
        }
    }

    public class TilemapTile
    {
        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }

    public class TexturedTile : TilemapTile
    {
        public int TileID { get; set; }

        public bool Collidable { get; set; }

        public Rectangle SourceRect { get; init; }

        public Rectangle WorldRect { get; init; }

        public Texture2D Texture { get; init; }

        public SpriteEffects SpriteEffects { get; init; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, WorldRect, SourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
