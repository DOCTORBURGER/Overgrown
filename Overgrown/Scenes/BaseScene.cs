using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overgrown.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overgrown.Scenes
{
    public abstract class BaseScene : IScene
    {
        public SceneManager SceneManager { get; set; }

        public virtual void LoadContent() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }
    }
}
