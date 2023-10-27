using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Collections.Generic;
using TilemapPipeline;
using static System.Net.Mime.MediaTypeNames;

namespace TiledPipeline
{
    [ContentProcessor(DisplayName = "Tilemap Processor")]
    internal class TilemapProcessor : ContentProcessor<TiledMapContent, TilemapContent>
    {
        private struct TileInfo
        {
            public ExternalReference<Texture2DContent> Texture;

            public Rectangle SourceRect;
        }

        public override TilemapContent Process(TiledMapContent input, ContentProcessorContext context)
        {
            TilemapContent output = new();

            List<TileInfo> tiles = ProcessTilesets(input.TileWidth, input.TileHeight, input.Tilesets, context);

            output.Layers = ProcessLayers(input.TileLayers, tiles, input.TileWidth, input.TileHeight, context).ToArray();

            output.Width = input.TileWidth * input.Width;

            output.Height = input.TileHeight * input.Height;

            return output;
        }

        private List<TileInfo> ProcessTilesets(int tileWidth, int tileHeight, List<TilesetContent> tilesets, ContentProcessorContext context)
        {
            List<TileInfo> processedTiles = new();

            foreach (TilesetContent tileset in tilesets)
            {
                ExternalReference<Texture2DContent> texture = context.BuildAsset<TextureContent, Texture2DContent>(new ExternalReference<TextureContent>(tileset.ImageFilename), "TextureProcessor");

                Texture2DContent image = context.BuildAndLoadAsset<TextureContent, Texture2DContent>(new ExternalReference<TextureContent>(tileset.ImageFilename), "TextureProcessor");

                int textureWidth = image.Mipmaps[0].Width;
                int textureHeight = image.Mipmaps[0].Height;

                int tilesetColumns = (textureWidth - 2 * tileset.Margin) / (tileWidth + tileset.Spacing);
                int tilesetRows = (textureHeight - 2 * tileset.Margin) / (tileHeight + tileset.Spacing);

                for (int y = 0; y < tilesetRows; y++)
                {
                    for (int x = 0; x < tilesetColumns; x++)
                    {
                        processedTiles.Add(new TileInfo()
                        {
                            Texture = texture,
                            SourceRect = new Rectangle(
                                x * (tileWidth + tileset.Spacing) + tileset.Margin,
                                y * (tileHeight + tileset.Spacing) + tileset.Margin,
                                tileWidth,
                                tileHeight
                            )
                        });
                    }
                }
            }

            return processedTiles;
        }

        private List<TilemapLayerContent> ProcessLayers(List<TiledLayerContent> layers, List<TileInfo> tileInfoList, int tileWidth, int tileHeight, ContentProcessorContext context)
        {
            List<TilemapLayerContent> processedLayers = new();

            foreach (var layer in layers)
            {
                List<TilemapTileContent> tiles = new();

                for (int y = 0; y < layer.Height; y++)
                {
                    for (int x = 0; x < layer.Width; x++)
                    {
                        int index = y * layer.Width + x;

                        int tileIndex = layer.TileIndices[index] - 1;

                        if (tileIndex == -1)
                        {
                            tiles.Add(new TilemapTileContent());
                            continue;
                        }

                        var tileInfo = tileInfoList[tileIndex];
                        tiles.Add(new TexturedTileContent()
                        {
                            Texture = tileInfo.Texture,
                            SourceRect = tileInfo.SourceRect,
                            WorldRect = new Rectangle()
                            {
                                X = x * tileWidth,
                                Y = y * tileHeight,
                                Width = tileWidth,
                                Height = tileHeight
                            },
                            SpriteEffects = layer.SpriteEffects[tileIndex]
                        });
                    }
                }

                processedLayers.Add(new TilemapLayerContent()
                {
                    Tiles = tiles.ToArray(),
                    Opacity = layer.Opacity
                });
            }

            return processedLayers;
        }
    }
}