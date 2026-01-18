using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mygame.Core.Collision;
using Mygame.Core.GameLoop;
using Mygame.Core.Combat;

namespace Mygame.Core.Entities.Enemy
{
    public class StaticEnemyEntity: IEntity, IDamaging, IKillable
    {
        public Vector2 Position { get; set; }
        public ICollider? Collider { get; }

        public bool IsDead { get; private set; }

        private readonly Texture2D _tex;
        private readonly int _w;
        private readonly int _h;

        public StaticEnemyEntity(Texture2D tex, Rectangle worldRect)
        {
            _tex = tex;
            Position = new Vector2(worldRect.X, worldRect.Y);
            _w = worldRect.Width;
            _h = worldRect.Height;

            // collider exact even groot als wat je tekent
            Collider = new RectCollider(() => Position, new Point(_w, _h), Point.Zero);
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            var dst = new Rectangle((int)Position.X, (int)Position.Y, _w, _h);
            spriteBatch.Draw(_tex, dst, Color.White);
        }

        public void Kill() => IsDead = true;
    }
}
