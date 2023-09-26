using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Managers;

namespace Overgrown.Scenes
{
    public interface IScene
    {
        SceneManager SceneManager { get; set; }

        void LoadContent();

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
