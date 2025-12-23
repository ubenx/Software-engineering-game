using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Mygame.Core.Collision;

namespace Mygame.Core.GameLoop
{
    public sealed class GameWorld
    {
        private readonly List<IEntity> _entities = new();
        public IReadOnlyList<IEntity> Entities => _entities;

        public CollisionSystem Collision { get; } = new();

        public void Add(IEntity entity)
        {
            _entities.Add(entity);

            if (entity.Collider != null)
                Collision.Register(entity);

            // Inject CollisionSystem in entities that need it (Player, enemies, projectiles...)
            if (entity is Mygame.Core.Collision.ICollisionAware aware)
                aware.SetCollision(Collision);
        }

        public void AddRange(IEnumerable<IEntity> entities)
        {
            foreach (var e in entities)
                Add(e);
        }

        public T? FindFirst<T>() where T : class, IEntity
            => _entities.OfType<T>().FirstOrDefault();

        public void Update(GameTime gameTime)
        {
            foreach (var e in _entities.ToList())
                e.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var e in _entities)
                e.Draw(spriteBatch);
        }
    }
}
