using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Overgrown
{
    public class OvergrownGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TitleSprite titleSprite;

        private ButtonSprite startButton;
        private ButtonSprite loadButton;
        private ButtonSprite optionsButton;
        private ButtonSprite exitButton;

        public OvergrownGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            titleSprite = new TitleSprite();
            startButton = new ButtonSprite("START", new Vector2(75, 75));
            loadButton = new ButtonSprite("LOAD", new Vector2(75 + 300 + 50, 75));
            optionsButton = new ButtonSprite("OPTIONS", new Vector2(75, 225));
            exitButton = new ButtonSprite("QUIT", new Vector2(75 + 300 + 50, 225));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            titleSprite.LoadContent(Content);
            startButton.LoadContent(Content);
            loadButton.LoadContent(Content);
            optionsButton.LoadContent(Content);
            exitButton.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            titleSprite.Draw(gameTime, _spriteBatch);
            startButton.Draw(_spriteBatch);
            loadButton.Draw(_spriteBatch);
            optionsButton.Draw(_spriteBatch);
            exitButton.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}