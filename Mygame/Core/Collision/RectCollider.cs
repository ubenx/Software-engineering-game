using Microsoft.Xna.Framework;
using System;


namespace Mygame.Core.Collision
{
    //bepaalt grootte van van Enemy of Player Hitbox
    public sealed class RectCollider: ICollider
    {
        
        private readonly System.Func<Vector2> _getPosition;


  
        public Point Size { get; }
        public Point Offset { get; }

        public RectCollider(System.Func<Vector2> getPosition, Point size, Point? offset = null)
        {
            _getPosition = getPosition;
            Size = size;
            Offset = offset ?? Point.Zero;
        }

        public Rectangle Bounds
        {
            get
            {
                var p = _getPosition();
                return new Rectangle(
                    (int)MathF.Round(p.X) + Offset.X,
                    (int)MathF.Round(p.Y) + Offset.Y,
                    Size.X,
                    Size.Y
                );
            }
        }
    }
}
