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
            public bool Collidable;

            public int TileID;

            public ExternalReference<Texture2DContent> Texture;

            public Rectangle SourceRect;

            public byte ExternalEdges;
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
                        int tileID = (y * tilesetColumns) + x;
                        bool collidable = false;
                        if (tileset.TileProperties.ContainsKey(tileID))
                        {
                            tileset.TileProperties.TryGetValue(tileID, out Dictionary<string, string> properties);
                            if (properties.ContainsKey("Collidable")) { collidable = true; }
                        }

                        processedTiles.Add(new TileInfo()
                        {
                            TileID = tileID,
                            Collidable = collidable,
                            Texture = texture,
                            SourceRect = new Rectangle(
                                x * (tileWidth + tileset.Spacing) + tileset.Margin,
                                y * (tileHeight + tileset.Spacing) + tileset.Margin,
                                tileWidth,
                                tileHeight
                            ),
                            ExternalEdges = 0
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
                            TileID = tileInfo.TileID,
                            Collidable = tileInfo.Collidable,
                            Texture = tileInfo.Texture,
                            SourceRect = tileInfo.SourceRect,
                            WorldRect = new Rectangle()
                            {
                                X = x * tileWidth,
                                Y = y * tileHeight,
                                Width = tileWidth,
                                Height = tileHeight
                            },
                            SpriteEffects = layer.SpriteEffects[tileIndex],
                            ExternalEdges = GetExternalEdges(x, y, layer, tileInfoList)
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

        private int GetExternalEdges(int x, int y, TiledLayerContent layer, List<TileInfo> tileInfoList)
        {
            byte externalEdges = 0;

            // Check top
            if (y == 0 || IsExternalEdge(x, y - 1, layer, tileInfoList, layer.Width))
            {
                externalEdges |= (byte)TileSides.Top;
            }

            // Check bottom
            if (y == layer.Height - 1 || IsExternalEdge(x, y + 1, layer, tileInfoList, layer.Width))
            {
                externalEdges |= (byte)TileSides.Bottom;
            }

            // Check left
            if (x == 0 || IsExternalEdge(x - 1, y, layer, tileInfoList, layer.Width))
            {
                externalEdges |= (byte)TileSides.Left;
            }

            // Check right
            if (x == layer.Width - 1 || IsExternalEdge(x + 1, y, layer, tileInfoList, layer.Width))
            {
                externalEdges |= (byte)TileSides.Right;
            }

            return externalEdges;
        }

        private bool IsExternalEdge(int x, int y, TiledLayerContent layer, List<TileInfo> tileInfoList, int layerWidth)
        {
            int index = y * layerWidth + x;
            int tileIndex = layer.TileIndices[index] - 1;

            // Check if the tile is outside the bounds or non-collidable
            return tileIndex == -1 || !tileInfoList[tileIndex].Collidable;
        }
    }
}