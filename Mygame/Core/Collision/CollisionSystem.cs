using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Mygame.Core.GameLoop;
using System;
using Mygame.Core.Physics;

namespace Mygame.Core.Collision
{
    public sealed class CollisionSystem
    {
        //hier wordt ook zwaartekracht in gemaakt

        public float Gravity = 2000f;      // pixels/sec^2
        public float MaxFallSpeed = 2500f; // cap


        private readonly List<IEntity> _collidables = new();


        //zwaartekracht
        public MoveResult ApplyPhysics(IEntity entity, IPhysicsBody body, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            body.Velocity = new Vector2(
                body.Velocity.X,
                MathF.Min(body.Velocity.Y + Gravity * dt, MaxFallSpeed)
            );

            float dx = body.Velocity.X * dt;
            float dy = body.Velocity.Y * dt;

            var pos = entity.Position;
            var result = MoveWithCollision(entity, ref pos, dx, dy);
            entity.Position = pos;

            if (result.HitBottom)
                body.Velocity = new Vector2(body.Velocity.X, 0);

            body.IsGrounded = result.HitBottom;
            return result;
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

                // alleen solids blokkeren movement
                if (e is not Mygame.Core.Collision.ISolid)
                    continue;

                if (e.Collider != null && e.Collider.Bounds.Intersects(rect))
                    return true;
            }
            return false;
        }

   
        public MoveResult MoveWithCollision(IEntity entity, ref Vector2 position, float dx, float dy)
        {
            var result = new MoveResult();

            MoveAxis(entity, ref position, dx, 0f, result); // X
            MoveAxis(entity, ref position, 0f, dy, result); // Y

            return result;

            
        }


        // Nodig voor strakke collisions
        private void MoveAxis(IEntity entity, ref Vector2 position, float dx, float dy, MoveResult result)
        {
            float amount = dx != 0 ? dx : dy;
            if (amount == 0) return;

            int dir = Math.Sign(amount);       // -1 of +1
            float abs = MathF.Abs(amount);

            int whole = (int)MathF.Floor(abs); // hele pixels
            float rem = abs - whole;           // remainder 0..1

            // 1) hele pixels stap-voor-stap
            for (int i = 0; i < whole; i++)
            {
                var nextPos = new Vector2(
                    position.X + (dx != 0 ? dir : 0),
                    position.Y + (dy != 0 ? dir : 0)
                );

                var nextRect = RectAt(entity, nextPos);

                if (!IntersectsAny(nextRect, ignore: entity))
                {
                    position = nextPos;
                }
                else
                {
                    SetHitFlags(dx, dy, dir, result);
                    return; // stop met bewegen op deze as
                }
            }
        }


        //Nodig voor strakke collisions
            private static void SetHitFlags(float dx, float dy, int dir, MoveResult result)
            {
                if (dx != 0)
                {
                    if (dir < 0) result.HitLeft = true;
                    else result.HitRight = true;
                }
                else if (dy != 0)
                {
                    if (dir < 0) result.HitTop = true;
                    else result.HitBottom = true;
                }
            }

            private static Rectangle RectAt(IEntity e, Vector2 pos)
            {
                if (e.Collider is RectCollider rc)
                {
                    return new Rectangle(
                        (int)MathF.Round(pos.X) + rc.Offset.X,
                        (int)MathF.Round(pos.Y) + rc.Offset.Y,
                        rc.Size.X,
                        rc.Size.Y
                    );
                }

                return e.Collider?.Bounds ?? Rectangle.Empty;
            }

        public void Unregister(IEntity entity)
        {
            _collidables.Remove(entity);
        }

    }
} 
