using Microsoft.Xna.Framework;


namespace Mygame.Core.Collision
{
    public sealed class RectCollider: ICollider
    {
        private readonly System.Func<Vector2> _getPosition;
        private readonly Point _size;
        private readonly Point _offset;

        public RectCollider(System.Func<Vector2> getPosition, Point size, Point? offset = null)
        {
            _getPosition = getPosition;
            _size = size;
            _offset = offset ?? Point.Zero;
        }

        public Rectangle Bounds
        {
            get
            {
                var p = _getPosition();
                return new Rectangle(
                    (int)p.X + _offset.X,
                    (int)p.Y + _offset.Y,
                    _size.X,
                    _size.Y
                );
            }
        }
    }
}
