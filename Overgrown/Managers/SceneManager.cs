using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overgrown.Managers
{
    public class SceneManager : DrawableGameComponent
    {
        private IScene currentScene;

        private readonly ContentManager content;

        public SpriteBatch SpriteBatch { get; private set; }

        public SpriteFont Font { get; private set; }

        public SceneManager(Game game) : base(game)
        {
            content = new ContentManager(game.Services, "Content");
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Font = content.Load<SpriteFont>("button_font");
        }

        public void SetScene(IScene newScene)
        {
            currentScene = newScene;
            newScene.SceneManager = this;
            currentScene.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            currentScene.Draw(gameTime);
        }
    }
}
