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
        private SunSprite sunSprite;
        private CloudSprite cloudSprite;

        private Button startButton;
        private Button loadButton;
        private Button optionsButton;
        private Button exitButton;

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
            cloudSprite = new CloudSprite();
            sunSprite= new SunSprite();
            startButton = new Button("START", new Vector2(75, 75));
            loadButton = new Button("LOAD", new Vector2(75 + 300 + 50, 75));
            optionsButton = new Button("OPTIONS", new Vector2(75, 225));
            exitButton = new Button("QUIT", new Vector2(75 + 300 + 50, 225));
            exitButton.Click += ExitButton_Click;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            titleSprite.LoadContent(Content);
            cloudSprite.LoadContent(Content);
            sunSprite.LoadContent(Content);
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
            cloudSprite.Update(gameTime, GraphicsDevice);
            exitButton.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            titleSprite.Draw(gameTime, _spriteBatch);
            sunSprite.Draw(gameTime, _spriteBatch);
            cloudSprite.Draw(gameTime, _spriteBatch);
            startButton.Draw(_spriteBatch);
            loadButton.Draw(_spriteBatch);
            optionsButton.Draw(_spriteBatch);
            exitButton.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ExitButton_Click(object sender, System.EventArgs e)
        {
            Exit();
        }
    }
}