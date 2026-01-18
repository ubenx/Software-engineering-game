
using Mygame.Core.Entities.Enemy;
using Mygame.Core.Entities.Player;
using Mygame.Core.GameLoop;


namespace Mygame.Core.Combat
{
    public class DamageSystem
    {
        public bool CheckPlayerHit(GameWorld world, PlayerEntity player)
        {
            if (player.Collider == null) return false;


            // Loop door alle damaging entities
            foreach (var dmg in world.FindAll<IDamaging>())
            {
                if (dmg is not IEntity e) continue;
                if (e.Collider == null) continue;

                // negeer dode enemies
                if (e is IKillable k && k.IsDead)
                    continue;

                // Check collision
                if (player.Collider.Bounds.Intersects(e.Collider.Bounds))
                    return true; // player is geraakt
            }

            return false;
        }

      

       

    }
}
