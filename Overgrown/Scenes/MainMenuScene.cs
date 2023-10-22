using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Overgrown.Entities;
using Overgrown.Managers;
using Overgrown.UI;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Overgrown.Scenes
{
    public class MainMenuScene : BaseScene
    {
        private Title title;

        private Button startButton;
        private Button loadButton;
        private Button optionsButton;
        private Button exitButton;
        private List<Button> buttons;

        private Sun sunSprite;
        private Cloud cloudSprite;

        private Song backgroundMusic;

        private ContentManager content;

        public MainMenuScene()
        {
            title = new Title();

            startButton = new Button("START", new Vector2(75, 75));
            startButton.Click += StartButton_Click;

            loadButton = new Button("LOAD", new Vector2(75 + 300 + 50, 75));

            optionsButton = new Button("OPTIONS", new Vector2(75, 225));

            exitButton = new Button("QUIT", new Vector2(75 + 300 + 50, 225));
            exitButton.Click += ExitButton_Click;

            buttons = new List<Button>
            {
                startButton,
                loadButton,
                optionsButton,
                exitButton
            };

            cloudSprite = new Cloud();
            sunSprite = new Sun();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            title.LoadContent(content);

            startButton.LoadContent(content);
            loadButton.LoadContent(content);
            optionsButton.LoadContent(content);
            exitButton.LoadContent(content);

            cloudSprite.LoadContent(content);
            sunSprite.LoadContent(content);

            backgroundMusic = content.Load<Song>("Geb - Sequoias Start as Saplings");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
        }

        public override void UnloadContent()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            exitButton.Click -= ExitButton_Click;

            content.Unload();
        }

        public override void Update(GameTime gameTime) 
        {
            base.Update(gameTime);

            cloudSprite.Update(gameTime, SceneManager.Game.GraphicsDevice);

            foreach (var button in buttons)
            {
                button.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) 
        {
            SpriteBatch spriteBatch = SceneManager.SpriteBatch;

            spriteBatch.Begin();

            title.Draw(gameTime, spriteBatch);

            sunSprite.Draw(gameTime, spriteBatch);

            cloudSprite.Draw(gameTime, spriteBatch);

            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private void ExitButton_Click(object sender, System.EventArgs e)
        {
            SceneManager.Game.Exit();
        }

        private void StartButton_Click(object sender, System.EventArgs e)
        {
            SceneManager.SetScene(new GameScene());
        }
    }
}
