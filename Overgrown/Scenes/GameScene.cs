using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Entities;
using Microsoft.Xna.Framework.Media;
using Overgrown.Managers;
using Overgrown.State_Management;
using System.IO;
using Overgrown.Tilemaps;

namespace Overgrown.Scenes
{
    public class GameScene : IScene
    {
        private ContentManager _content;

        private Player _player;

        private Song _backgroundMusic;

        private Tilemap _map;

        public SceneManager SceneManager { get; set; }

        public GameScene()
        {
            _player = new Player();
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
        }

        public void UnloadContent()
        {
            if (_content != null)
                _content.Unload();
        }

        public void HandleInput(GameTime gameTime, InputState input)
        {

        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SceneManager.SpriteBatch;

            spriteBatch.Begin();

            _map.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
