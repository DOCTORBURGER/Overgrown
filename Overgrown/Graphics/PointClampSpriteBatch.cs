using Microsoft.Xna.Framework.Graphics;

namespace Overgrown.Graphics
{
    public class PointClampSpriteBatch : SpriteBatch
    {
        public PointClampSpriteBatch(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }

        public void Begin()
        {
            base.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
        }
    }
}
