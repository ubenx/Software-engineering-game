using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Mygame.Core.GameLoop;

namespace Mygame.Core.Collision
{
    public sealed class CollisionSystem
    {
        private readonly List<IEntity> _collidables = new();

        public void Register(IEntity entity)
        {
            if (!_collidables.Contains(entity))
                _collidables.Add(entity);
        }

        public bool IntersectsAny(Rectangle rect, IEntity? ignore = null)
        {
            foreach (var e in _collidables)
            {
                if (ignore != null && ReferenceEquals(e, ignore))
                    continue;

                if (e.Collider != null && e.Collider.Bounds.Intersects(rect))
                    return true;
            }
            return false;
        }

        public void MoveWithCollision(IEntity entity, ref Vector2 position, float dx, float dy)
        {
            // X
            if (dx != 0)
            {
                var nextPos = new Vector2(position.X + dx, position.Y);
                var nextRect = RectAt(entity, nextPos);

                if (!IntersectsAny(nextRect, ignore: entity))
                    position = nextPos;
            }

            // Y
            if (dy != 0)
            {
                var nextPos = new Vector2(position.X, position.Y + dy);
                var nextRect = RectAt(entity, nextPos);

                if (!IntersectsAny(nextRect, ignore: entity))
                    position = nextPos;
            }
        }

        private static Rectangle RectAt(IEntity e, Vector2 pos)
        {
            if (e.Collider is not RectCollider)
            {
                // Fallback: if collider isn't RectCollider, we can't easily offset it.
                // In this project we use RectCollider everywhere.
                return e.Collider?.Bounds ?? Rectangle.Empty;
            }

            // We recompute by temporarily deriving from bounds size and origin at pos.
            // Assumption: offset = 0 for simplicity.
            var b = e.Collider!.Bounds;
            return new Rectangle((int)pos.X, (int)pos.Y, b.Width, b.Height);
        }
    }
}
