using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace TiledPipeline
{
    public enum TileSides : byte
    {
        None = 0,
        Top = 1,    // 0001
        Bottom = 2, // 0010
        Left = 4,   // 0100
        Right = 8   // 1000
    }

    [ContentSerializerRuntimeType("Overgrown.Tilemaps.Tilemap, Overgrown")]
    public class TilemapContent
    {
        public TilemapLayerContent[] Layers { get; set; }

        public int Width;

        public int Height;
    }

    [ContentSerializerRuntimeType("Overgrown.Tilemaps.TilemapLayer, Overgrown")]
    public class TilemapLayerContent
    {
        public TilemapTileContent[] Tiles { get; set; }

        public float Opacity { get; set; } = 1.0f;

        public bool Visible { get; set; } = true;
    }

    [ContentSerializerRuntimeType("Overgrown.Tilemaps.TilemapTile, Overgrown")]
    public class TilemapTileContent
    {

    }

    [ContentSerializerRuntimeType("Overgrown.Tilemaps.TexturedTile, Overgrown")]
    public class TexturedTileContent : TilemapTileContent
    {
        public int TileID { get; set; } = 2;

        public bool Collidable { get; set; }

        public Rectangle SourceRect { get; set; }

        public Rectangle WorldRect { get; set; }

        public ExternalReference<Texture2DContent> Texture { get; init; }

        public SpriteEffects SpriteEffects { get; init; }

        public int ExternalEdges { get; set; }
    }
}
