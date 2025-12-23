using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mygame.Core.Collision;
using Mygame.Core.GameLoop;
using Mygame.Core.Rendering;


namespace Mygame.Core.Entities
{
    public class BlockEntity: IEntity
    {
        public Vector2 Position { get; set; }

        public ICollider Collider { get; }

        private readonly SpriteRenderer _renderer;
        private readonly int _w;
        private readonly int _h;

        public BlockEntity(Texture2D texture, Vector2 position, int width, int height)
        {
            Position = position;
            _w = width;
            _h = height;

            _renderer = new SpriteRenderer(texture);
            Collider = new RectCollider(() => Position, new Point(width, height));
        }

        public void Update(GameTime gameTime)
        {
            // Static block -> no logic
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var rect = new Rectangle((int)Position.X, (int)Position.Y, _w, _h);
            _renderer.Draw(spriteBatch, rect, Color.White);
        }
    }
}
