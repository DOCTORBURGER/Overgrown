namespace Overgrown.Collisions
{
    public class CollisionHelper
    {
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right ||
                a.Top > b.Bottom || a.Bottom < b.Top);
        }
    }
}
