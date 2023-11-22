using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overgrown.Entities;
using Microsoft.Xna.Framework.Media;
using Overgrown.Managers;
using Overgrown.State_Management;
using Overgrown.Tilemaps;
using Overgrown.Save_System;
using System.Collections.Generic;
using Overgrown.Collisions;
using System.Transactions;
using System;

namespace Overgrown.Scenes
{
    public class GameScene : IScene
    {
        private ContentManager _content;

        private Player _player;

        private Song _backgroundMusic;

        private Tilemap _map;

        private Camera _camera;

        private readonly InputAction _saveGame;

        public SceneManager SceneManager { get; set; }

        public GameScene()
        {
            _player = new Player();
            Vector2? position = SaveSystem.Load();
            if (position.HasValue) { _player.Position = position.Value; }

            _saveGame = new InputAction(new[] { Keys.Enter }, true);
        }

        public void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(SceneManager.Game.Services, "Content");

            _player.LoadContent(_content);
            _backgroundMusic = _content.Load<Song>("SoundTracks/Kevin MacLeod - Erik Satie_ Gymnopedie No 1");
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.IsRepeating = true;

            _map = _content.Load<Tilemap>("Tilemaps/TileMapTemp");

            _camera = new Camera(SceneManager.VirtualResolution, _map);
        }

        public void UnloadContent()
        {
            if (_content != null)
                _content.Unload();
        }

        public void HandleInput(GameTime gameTime, InputState input)
        {
            if (_saveGame.Occurred(input))
            {
                SaveSystem.Save(_player);
            }
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            CheckCollisions();
            _camera.Follow(_player.Position);
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SceneManager.SpriteBatch;

            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(-_camera.Position, 0)));

            _map.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void CheckCollisions()
        {
            BoundingRectangle playerBounds = _player.Bounds;

            List<TexturedTile> tilesToCheck = GetNearbyCollidableTiles(playerBounds);

            foreach (TexturedTile tile in tilesToCheck)
            {
                if (tile.Collidable)
                {
                    if (CollisionHelper.Collides(playerBounds, new BoundingRectangle(tile.WorldRect.X, tile.WorldRect.Y, tile.WorldRect.Width, tile.WorldRect.Height)))
                    {
                        // Handle the collision with the tile
                        ResolveCollision(_player, tile);
                    }
                }
            }
        }

        private List<TexturedTile> GetNearbyCollidableTiles(BoundingRectangle playerBounds)
        {
            List<TexturedTile> nearbyTiles = new List<TexturedTile>();

            int boundsTileX = (int)(playerBounds.X / 32);
            int boundsTileY = (int)(playerBounds.Y / 32);

            int boundsTileWidth = (int)(playerBounds.Width / 32);
            int boundsTileHeight = (int)(playerBounds.Height / 32);

            foreach (TilemapLayer layer in _map.Layers)
            {
                for (int x = boundsTileX - 1; x < boundsTileX + boundsTileWidth + 1; x++)
                {
                    if (x > _map.Width / 32 || x < 0)
                    {
                        continue;
                    }

                    for (int y = boundsTileY - 1; y < boundsTileY + boundsTileHeight + 1; y++)
                    {
                        if (y >= (_map.Height / 32) || y < 0)
                        {
                            continue;
                        }

                        int index = (y * (_map.Width / 32)) + x;
                        
                        if (layer.Tiles[index] is TexturedTile texturedTile)
                        {
                            nearbyTiles.Add(texturedTile);
                        }
                    }
                }
            }
            

            return nearbyTiles;
        }

        private void ResolveCollision(Player player, TexturedTile tile)
        {
            BoundingRectangle playerBounds = player.Bounds;
            BoundingRectangle tileBounds = new BoundingRectangle(
                tile.WorldRect.X, tile.WorldRect.Y, tile.WorldRect.Width, tile.WorldRect.Height);

            if (!CollisionHelper.Collides(tileBounds, playerBounds))
            {
                return;
            }

            float topOverlap = (playerBounds.Bottom - tileBounds.Top);
            float bottomOverlap = (tileBounds.Bottom - playerBounds.Top);
            float leftOverlap = (playerBounds.Right - tileBounds.Left);
            float rightOverlap = (tileBounds.Right - playerBounds.Left);

            float minOverlap = Math.Min(Math.Min(topOverlap, bottomOverlap), Math.Min(leftOverlap, rightOverlap));

            Vector2 position = player.Position;
            Vector2 velocity = player.Velocity;

            if (minOverlap == topOverlap)
            {
                player.Grounded = true;
                position.Y -= topOverlap;
                velocity.Y = 0;
            }
            else if (minOverlap == bottomOverlap)
            {
                position.Y += bottomOverlap + 1;
                velocity.Y = 0;
            }
            else if (minOverlap == leftOverlap)
            {
                position.X -= leftOverlap;
                velocity.X = 0;
            }
            else if (minOverlap == rightOverlap)
            {
                position.X += rightOverlap;
                velocity.X = 0;
            }

            player.Position = position;
            player.Velocity = velocity;

            player.UpdateBounds();
        }
    }
}
