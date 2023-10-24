using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Managers;
using Overgrown.State_Management;

namespace Overgrown.Scenes
{
    public interface IScene
    {
        SceneManager SceneManager { get; set; }

        void LoadContent();

        void UnloadContent();

        void HandleInput(GameTime gameTime, InputState input);

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
