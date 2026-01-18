using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Mygame.Core.Collision;
using Mygame.Core.Physics;
using Mygame.Core.Entities.Enemy;

namespace Mygame.Core.GameLoop
{
    public sealed class GameWorld
    {
        private readonly List<IEntity> _entities = new();
      

        public CollisionSystem Collision { get; } = new();


        public void Add(IEntity entity)
        {
            _entities.Add(entity);

            if (entity.Collider != null)
                Collision.Register(entity);

            //// Inject CollisionSystem in entities that need it (Player, enemies, projectiles...)
            //if (entity is Mygame.Core.Collision.ICollisionAware aware)
            //    aware.SetCollision(Collision);
        }

        public void Remove(IEntity entity)
        {
            _entities.Remove(entity);
            if (entity.Collider != null)
                Collision.Unregister(entity);

        }


        public void AddRange(IEnumerable<IEntity> entities)
        {
            foreach (var e in entities)
                Add(e);
        }

        public T? FindFirst<T>() where T : class, IEntity
            => _entities.OfType<T>().FirstOrDefault();

        public IEnumerable<T> FindAll<T>() where T : class => _entities.OfType<T>();


        public void Update(GameTime gameTime)
        {
            //foreach (var e in _entities.ToList())
            //    e.Update(gameTime);

            // 1) intent/AI/anim
            foreach (var e in _entities)
                e.Update(gameTime);

            // 2) physics + collision (gravity)
            foreach (var e in _entities)
            {
                if (e is IPhysicsBody body)
                {
                    var res = Collision.ApplyPhysics(e, body, gameTime);

                    // patrol enemy gebruikt wall hits
                    if (e is PatrolEnemyEntity pe)
                        pe.LastMove = res;
                }
            }

            //Vijand verdwijnt
            // Vijand verdwijnt (ook uit CollisionSystem)
            var dead = _entities
                .Where(e => e is Mygame.Core.Combat.IKillable k && k.IsDead)
                .ToList();

            foreach (var e in dead)
                Remove(e);



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var e in _entities)
                e.Draw(spriteBatch);
        }
    }
}
