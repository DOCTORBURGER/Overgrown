using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overgrown.UI;
using Overgrown.Entities;
using System.Collections.Generic;
using Overgrown.Managers;
using Overgrown.Scenes;

namespace Overgrown
{
    public class OvergrownGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SceneManager sceneManager;

        public OvergrownGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            sceneManager = new SceneManager(this, _graphics);
            Components.Add(sceneManager);
        }

        protected override void Initialize()
        {
            sceneManager.SetScene(new MainMenuScene());

            base.Initialize();
        }

        protected override void LoadContent() { }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            base.Draw(gameTime);
        }
    }
}