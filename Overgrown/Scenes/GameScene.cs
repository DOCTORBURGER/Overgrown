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
    }
}
