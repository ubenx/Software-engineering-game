using Microsoft.Xna.Framework;
using Mygame.Core.Entities.Enemy;
using Mygame.Core.Entities.Player;
using Mygame.Core.GameLoop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mygame.Core.Combat
{
    public class DamageSystem
    {
        public bool CheckPlayerHit(GameWorld world, PlayerEntity player)
        {
            if (player.Collider == null) return false;

            foreach (var dmg in world.FindAll<IDamaging>())
            {
                if (dmg is not IEntity e) continue;
                if (e.Collider == null) continue;

                // negeer dode enemies
                if (e is IKillable k && k.IsDead)
                    continue;

                if (player.Collider.Bounds.Intersects(e.Collider.Bounds))
                    return true;
            }

            return false;
        }

        private static bool TouchOrOverlap(Rectangle a, Rectangle b)
        {
            return a.Left <= b.Right &&
                   a.Right >= b.Left &&
                   a.Top <= b.Bottom &&
                   a.Bottom >= b.Top;
        }

       

    }
}
