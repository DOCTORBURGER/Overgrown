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
        private RenderTarget2D _renderTarget;
        private Matrix _scaleMatrix = Matrix.Identity;

        private readonly InputState _input = new InputState();

        public SpriteBatch SpriteBatch { get; private set; }

        public SpriteFont Font { get; private set; }

        public readonly Point VirtualResolution = new Point(854, 480);
        public Matrix ScaleMatrix => _scaleMatrix;

        public SceneManager(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            _content = new ContentManager(game.Services, "Content");
            _graphics = graphics;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            _renderTarget = new RenderTarget2D(
                GraphicsDevice,
                VirtualResolution.X,
                VirtualResolution.Y,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            Font = _content.Load<SpriteFont>("Silkscreen");

            CalculateMatrix();
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
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            _currentScene.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.DeepSkyBlue);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, ScaleMatrix);
            SpriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
            SpriteBatch.End();
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

        public int ReturnMaxWidth()
        {
            return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        }

        public bool SetFullScreen()
        {
            _graphics.ToggleFullScreen();

            if (_graphics.IsFullScreen)
            {
                // When going to fullscreen, set to the default monitor resolution
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }

            _graphics.ApplyChanges();
            CalculateMatrix();

            return _graphics.IsFullScreen;
        }
    }
}
