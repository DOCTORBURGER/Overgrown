using Microsoft.Xna.Framework;
using Overgrown.Tilemaps;

namespace Overgrown.Managers
{
    public class Camera
    {
        public Vector2 Position { get; private set; }
        private Point _virtualResolution;
        private Vector2 _minScroll, _maxScroll;

        public Camera(Point virtualResolution, Tilemap map)
        {
            _virtualResolution = virtualResolution;
            Position = Vector2.Zero;
            _minScroll = Vector2.Zero;
            _maxScroll = new Vector2(map.Width - (_virtualResolution.X), map.Height - _virtualResolution.Y);
        }

        public void Follow(Vector2 target)
        {
            Position = new Vector2((int)(target.X - (_virtualResolution.X / 2)), (int)(target.Y - (_virtualResolution.Y / 2)));
            Position = Vector2.Clamp(Position, _minScroll, _maxScroll);
        }
    }
}
