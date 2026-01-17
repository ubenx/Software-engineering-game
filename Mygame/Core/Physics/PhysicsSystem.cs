using Microsoft.Xna.Framework;
using Mygame.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mygame.Core.Physics
{
    public class PhysicsSystem
    {
        //    public float Gravity = 2000f;      // pixels/sec^2
        //    public float MaxFallSpeed = 2500f; // cap

        //    public void ApplyPhysics(PlayerEntity e, GameTime gameTime)
        //    {
        //        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        //        // gravity
        //        e.Velocity = new Vector2(
        //            e.Velocity.X,
        //            MathF.Min(e.Velocity.Y + Gravity * dt, MaxFallSpeed)
        //        );

        //        // move with collision using velocity * dt
        //        float dx = e.Velocity.X * dt;
        //        float dy = e.Velocity.Y * dt;

        //        var pos = e.Position;
        //        var result = MoveWithCollision(e, ref pos, dx, dy); // zie noot hieronder
        //        e.Position = pos;

        //        // als je op vloer botst, stop val-snelheid
        //        if (result.HitBottom) e.Velocity = new Vector2(e.Velocity.X, 0);
        //        e.IsGrounded = result.HitBottom;
        //    }

        //}
    }
}
