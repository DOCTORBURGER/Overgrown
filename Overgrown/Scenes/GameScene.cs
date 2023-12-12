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
        private Enemy _enemy;
        private Song _backgroundMusic;
        private Tilemap _map;
        private Camera _camera;

        private float[] _parallaxSpeeds = { 0.05f, 0.15f, 0.3f, 0.5f };

        private Texture2D[] _backgroundTexture = new Texture2D[4];

        private readonly InputAction _saveGame;

        public SceneManager SceneManager { get; set; }

        public GameScene()
        {
            _player = new Player();
            _enemy = new Enemy(new Vector2(2208, 97));
            Vector2? position = SaveSystem.Load();
            if (position.HasValue) { _player.Position = position.Value; }

            _saveGame = new InputAction(new[] { Keys.Enter }, true);
        }

        public void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(SceneManager.Game.Services, "Content");

            _player.LoadContent(_content);
            _enemy.LoadContent(_content);
            _backgroundMusic = _content.Load<Song>("SoundTracks/Kevin MacLeod - Erik Satie_ Gymnopedie No 1");
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.IsRepeating = true;

            _map = _content.Load<Tilemap>("Tilemaps/TileMapTemp");

            _camera = new Camera(SceneManager.VirtualResolution, _map);

            _backgroundTexture[0] = _content.Load<Texture2D>("Sprites/sky");
            _backgroundTexture[1] = _content.Load<Texture2D>("Sprites/backmostmountain");
            _backgroundTexture[2] = _content.Load<Texture2D>("Sprites/backmidmountain");
            _backgroundTexture[3] = _content.Load<Texture2D>("Sprites/midmountain");
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

            if (CollisionHelper.Collides(_player.Bounds, _enemy.Bounds) && _enemy.Collected == false)
            {
                _enemy.Collected = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SceneManager.SpriteBatch;
            spriteBatch.Begin();

            for (int i = 0; i < _backgroundTexture.Length; i++)
            {
                float parallaxFactor = _parallaxSpeeds[i]; 
                                                                                    
                float remainder = (_camera.Position.X * parallaxFactor) % _backgroundTexture[i].Width;
                float drawPosX = -remainder;

                if (remainder > 0)
                {
                    drawPosX -= _backgroundTexture[i].Width;
                }

                while (drawPosX < SceneManager.VirtualResolution.X)
                {
                    spriteBatch.Draw(_backgroundTexture[i], new Vector2(drawPosX, 0), Color.White);
                    drawPosX += _backgroundTexture[i].Width;
                }
            }

            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(-_camera.Position, 0)));

            _map.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            _enemy.Draw(spriteBatch);

            string objective = "Find the ugly guy!";
            if (_enemy.Collected) { objective = "You win!";  }
            spriteBatch.DrawString(SceneManager.Font, objective, new Vector2(_camera.Position.X, _camera.Position.Y), Color.White);

            spriteBatch.End();
        }

        private void CheckCollisions()
        {
            BoundingRectangle playerBounds = _player.Bounds;

            List<TexturedTile> tilesToCheck = GetNearbyCollidableTiles(playerBounds);

            _player.Grounded = false;

            foreach (TexturedTile tile in tilesToCheck)
            {
                if (tile.Collidable)
                {
                    if (CollisionHelper.Collides(playerBounds, new BoundingRectangle(tile.WorldRect.X, tile.WorldRect.Y, tile.WorldRect.Width, tile.WorldRect.Height)))
                    {
                        ResolveCollision(_player, tile);
                    }
                }
            }

            Vector2 position = _player.Position;
            Vector2 velocity = _player.Velocity;

            if (position.X < 0 + (playerBounds.Width / 2)) position.X = 0 + (playerBounds.Width / 2);
            if (position.X > (_map.Width) - (playerBounds.Width / 2)) position.X = 1600 - (playerBounds.Width / 2);
            if (position.Y < 0 + (playerBounds.Height / 2)) { position.Y = 0 + (playerBounds.Height / 2); velocity.Y = 0; }
            if (position.Y > (_map.Height) - (playerBounds.Height / 2)) { position.Y = 640 - (playerBounds.Height / 2); velocity.Y = 0; }

            _player.Position = position;
            _player.Velocity = velocity;
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
                        
                        if (layer.Tiles[index] is TexturedTile texturedTile && texturedTile.Collidable == true)
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

            int externalEdges = tile.ExternalEdges;

            if (minOverlap == topOverlap && (externalEdges & (int)TileSides.Top) == (int)TileSides.Top)
            {
                player.Grounded = true;
                position.Y -= (int)topOverlap;
                velocity.Y = 0;
            }
            else if (minOverlap == bottomOverlap && (externalEdges & (int)TileSides.Bottom) == (int)TileSides.Bottom)
            {
                position.Y += (int)bottomOverlap;
                if (velocity.Y < 0) velocity.Y = 0;
            }
            else if (minOverlap == leftOverlap && (externalEdges & (int)TileSides.Left) == (int)TileSides.Left)
            {
                position.X -= (int)leftOverlap;
            }
            else if (minOverlap == rightOverlap && (externalEdges & (int)TileSides.Right) == (int)TileSides.Right)
            {
                position.X += (int)rightOverlap;
            }

            player.Position = position;
            player.Velocity = velocity;

            player.UpdateBounds();
        }
    }
}
