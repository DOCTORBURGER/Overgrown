using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace TiledPipeline
{
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
        public Rectangle SourceRect { get; set; }

        public Rectangle WorldRect { get; set; }

        public ExternalReference<Texture2DContent> Texture { get; init; }

        public SpriteEffects SpriteEffects { get; init; }
    }
}
