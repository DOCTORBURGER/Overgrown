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
            _backgroundMusic = _content.Load<Song>("Kevin MacLeod - Erik Satie_ Gymnopedie No 1");
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.IsRepeating = true;

            _map = _content.Load<Tilemap>("TileMapTemp");

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
            // Implement collision resolution, adjusting the player's position and possibly their velocity.
            // You can implement different responses depending on the type of the tile (e.g., solid block, water, spikes).

            // Simple example: If the tile is solid, prevent the player from moving into the tile's space.
            Rectangle intersection = Rectangle.Intersect(
                new Rectangle((int)player.Bounds.X, (int)player.Bounds.Y, (int)player.Bounds.Width, (int)player.Bounds.Height),
                tile.WorldRect);

            if (intersection.Width < intersection.Height)
            {
                // Horizontal collision
                if (player.Position.X < tile.WorldRect.Center.X) // Player is on the left
                    player.Position = new Vector2(player.Position.X - intersection.Width, player.Position.Y);
                else // Player is on the right
                    player.Position = new Vector2(player.Position.X + intersection.Width, player.Position.Y);
            }
            else
            {
                // Vertical collision
                if (player.Position.Y < tile.WorldRect.Center.Y) // Player is above
                    player.Position = new Vector2(player.Position.X, player.Position.Y - intersection.Height);
                else // Player is below
                    player.Position = new Vector2(player.Position.X, player.Position.Y + intersection.Height);
            }

            // Update player bounds after resolving collisions
            player.UpdateBounds();
        }
    }
}
