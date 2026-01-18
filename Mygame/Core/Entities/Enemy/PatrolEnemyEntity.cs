using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mygame.Core.Collision;
using Mygame.Core.GameLoop;
using Mygame.Core.Physics;

using Mygame.Core.Combat;

namespace Mygame.Core.Entities.Enemy
{
    public class PatrolEnemyEntity: IEntity, IDamaging, IPhysicsBody, IKillable
    {
        public Vector2 Position { get; set; }
        public ICollider? Collider { get; }

        public Vector2 Velocity { get; set; }
        public bool IsGrounded { get; set; }

        public bool IsDead { get; private set; }

        public MoveResult LastMove { get; set; } = new MoveResult();

        private readonly Texture2D _tex;
        private readonly int _w;
        private readonly int _h;

        private float _dir = 1f;
        private readonly float _speed;

        public PatrolEnemyEntity(Texture2D tex, Vector2 pos, int drawW, int drawH, int hitW, int hitH, int offX, int offY, float speed = 140f)
        {
            _tex = tex;
            Position = pos;
            _w = drawW;
            _h = drawH;

            _speed = speed;

            Collider = new RectCollider(() => Position, new Point(hitW, hitH), new Point(offX, offY));
        }


        public PatrolEnemyEntity(Texture2D tex, Rectangle worldRect, float speed = 140f)
        {
            _tex = tex;

            Position = new Vector2(worldRect.X, worldRect.Y);
            _w = worldRect.Width;
            _h = worldRect.Height;

            _speed = speed;

            int hitW = (int)(_w * 0.70f);
            int hitH = (int)(_h * 0.85f);

            int offX = (_w - hitW) / 2;
            int offY = (_h - hitH);

            Collider = new RectCollider(() => Position, new Point(hitW, hitH), new Point(offX, offY));
        }


        public void Kill() => IsDead = true;
        public void Update(GameTime gameTime)
        {
            // keer om bij muur
            if (LastMove.HitLeft) _dir = 1f;
            if (LastMove.HitRight) _dir = -1f;

            Velocity = new Vector2(_dir * _speed, Velocity.Y);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var dst = new Rectangle((int)Position.X, (int)Position.Y, _w, _h);
            spriteBatch.Draw(_tex, dst, Color.White);
        }
    }
}
