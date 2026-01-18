using Microsoft.Xna.Framework;
using Mygame.Core.Entities.Player;
using Mygame.Core.GameLoop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mygame.Core.Combat
{
    public class StompSystem
    {
        // hoeveel pixels overlap tolerantie om “van boven” te tellen
        private const int VerticalTolerance = 12;

        public bool TryStomp(GameWorld world, PlayerEntity player)
        {
            if (player.Collider == null) return false;

            var p = player.Collider.Bounds;

            foreach (var enemy in world.FindAll<IEntity>())
            {
                if (enemy.Collider == null) continue;
                if (enemy is not IKillable killable) continue;
                if (killable.IsDead) continue;

                var e = enemy.Collider.Bounds;

                // Moeten raken (touch/overlap)
                bool touch =
                    p.Left <= e.Right && p.Right >= e.Left &&
                    p.Top <= e.Bottom && p.Bottom >= e.Top;

                if (!touch) continue;

                // "van boven": player bottom zit boven enemy top (met tolerantie)
                bool fromAbove = p.Bottom <= e.Top + VerticalTolerance;

                // player moet naar beneden gaan (falling)
                bool fallingOrLanding = player.Velocity.Y >= 0f;

                if (fromAbove && fallingOrLanding)
                {
                    killable.Kill();

                    // kleine bounce zodat het goed voelt
                    player.Velocity = new Vector2(player.Velocity.X, -250f);
                    player.IsGrounded = false;

                    return true;
                }
            }

            return false;
        }
    }
}
