using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Entities;
using Overgrown.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overgrown.Scenes
{
    public class GameScene : BaseScene
    {
        private Player player;
        private ContentManager content;

        public GameScene()
        {
            player = new Player();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            player.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            PointClampSpriteBatch spriteBatch = SceneManager.SpriteBatch;

            spriteBatch.Begin();

            player.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
