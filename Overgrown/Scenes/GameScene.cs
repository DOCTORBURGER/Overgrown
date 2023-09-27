using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Entities;
using Overgrown.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overgrown.Scenes
{
    public class GameScene : BaseScene
    {
        private ContentManager content;

        private Player player;

        private Song backgroundMusic;

        public GameScene()
        {
            player = new Player();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            player.LoadContent(content);
            backgroundMusic = content.Load<Song>("Kevin MacLeod - Erik Satie_ Gymnopedie No 1");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
        }

        public override void UnloadContent()
        {
            if (content != null)
                content.Unload();
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
