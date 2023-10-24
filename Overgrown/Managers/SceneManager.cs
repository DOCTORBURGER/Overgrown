using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Scenes;
using Overgrown.State_Management;

namespace Overgrown.Managers
{
    public class SceneManager : DrawableGameComponent
    {
        private IScene _currentScene;

        private readonly ContentManager _content;
        private GraphicsDeviceManager _graphics;
        private Matrix _scaleMatrix = Matrix.Identity;

        private readonly InputState _input = new InputState();

        public SpriteBatch SpriteBatch { get; private set; }

        public SpriteFont Font { get; private set; }

        public readonly Point VirtualResolution = new Point(800, 480);
        public Matrix ScaleMatrix => _scaleMatrix;

        public SceneManager(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            _content = new ContentManager(game.Services, "Content");
            _graphics = graphics;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Font = _content.Load<SpriteFont>("Silkscreen");
        }

        public void SetScene(IScene newScene)
        {
            if (_currentScene != null) { _currentScene.UnloadContent(); }

            _currentScene = newScene;
            newScene.SceneManager = this;

            _currentScene.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _input.Update();

            _currentScene?.HandleInput(gameTime, _input);
            _currentScene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _currentScene.Draw(gameTime);
        }

        public void CalculateMatrix()
        {
            float scaleX = (float)Game.GraphicsDevice.Viewport.Width / VirtualResolution.X;
            float scaleY = (float)Game.GraphicsDevice.Viewport.Height / VirtualResolution.Y;

            _scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }

        public void SetResolution(Point resolution)
        {
            _graphics.PreferredBackBufferWidth = resolution.X;
            _graphics.PreferredBackBufferHeight = resolution.Y;
            _graphics.ApplyChanges();

            CalculateMatrix();
        }

        public bool SetFullScreen()
        {
            _graphics.ToggleFullScreen();

            _graphics.ApplyChanges();

            return _graphics.IsFullScreen;
        }
    }
}
