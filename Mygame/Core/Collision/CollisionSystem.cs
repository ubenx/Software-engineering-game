using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Mygame.Core.GameLoop;
using Mygame.Core.Entities;
using System;

namespace Mygame.Core.Collision
{
    public sealed class CollisionSystem
    {
        //hier wordt ook zwaartekracht in gemaakt
        
        public float Gravity = 2000f;      // pixels/sec^2
        public float MaxFallSpeed = 2500f; // cap


        private readonly List<IEntity> _collidables = new();


        //zwaartekracht
        public void ApplyPhysics(PlayerEntity e, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // gravity
            e.Velocity = new Vector2(
                e.Velocity.X,
                MathF.Min(e.Velocity.Y + Gravity * dt, MaxFallSpeed)
            );

            // move with collision using velocity * dt
            float dx = e.Velocity.X * dt;
            float dy = e.Velocity.Y * dt;

            var pos = e.Position;
            var result = MoveWithCollision(e, ref pos, dx, dy); // zie noot hieronder
            e.Position = pos;

            // als je op vloer botst, stop val-snelheid
            if (result.HitBottom) e.Velocity = new Vector2(e.Velocity.X, 0);
            e.IsGrounded = result.HitBottom;
        }


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

        //public void MoveWithCollision(IEntity entity, ref Vector2 position, float dx, float dy)
        //{
        //    // X
        //    if (dx != 0)
        //    {
        //        var nextPos = new Vector2(position.X + dx, position.Y);
        //        var nextRect = RectAt(entity, nextPos);

        //        if (!IntersectsAny(nextRect, ignore: entity))
        //            position = nextPos;
        //    }

        //    // Y
        //    if (dy != 0)
        //    {
        //        var nextPos = new Vector2(position.X, position.Y + dy);
        //        var nextRect = RectAt(entity, nextPos);

        //        if (!IntersectsAny(nextRect, ignore: entity))
        //            position = nextPos;
        //    }
        //}
        public MoveResult MoveWithCollision(IEntity entity, ref Vector2 position, float dx, float dy)
        {
            var result = new MoveResult();

            // X
            if (dx != 0)
            {
                var nextPos = new Vector2(position.X + dx, position.Y);
                var nextRect = RectAt(entity, nextPos);

                if (!IntersectsAny(nextRect, ignore: entity))
                    position = nextPos;
                else
                {
                    if (dx < 0) result.HitLeft = true;
                    if (dx > 0) result.HitRight = true;
                }
            }

            // Y
            if (dy != 0)
            {
                var nextPos = new Vector2(position.X, position.Y + dy);
                var nextRect = RectAt(entity, nextPos);

                if (!IntersectsAny(nextRect, ignore: entity))
                    position = nextPos;
                else
                {
                    if (dy < 0) result.HitTop = true;
                    if (dy > 0) result.HitBottom = true;
                }
            }

            return result;
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
