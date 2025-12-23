using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mygame.Core.Collision;
using Mygame.Core.GameLoop;

namespace Mygame.Core.Entities
{
    public sealed class CollisionBlockEntity: IEntity
    {
        public Vector2 Position { get; set; }
        public ICollider Collider { get; }

        private readonly int _w;
        private readonly int _h;

        public CollisionBlockEntity(Rectangle rect)
        {
            Position = new Vector2(rect.X, rect.Y);
            _w = rect.Width;
            _h = rect.Height;

            Collider = new RectCollider(() => Position, new Point(_w, _h));
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            // onzichtbaar: teken niets duhhhhh
        }
    }
}
