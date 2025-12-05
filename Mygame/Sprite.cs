using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mygame
{
    internal abstract class Sprite
    {
        protected Texture2D texture;
        protected Vector2 position;

        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        // Virtuele Rect property voor collision/tekenen
        public virtual Rectangle Rect
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void Update(GameTime gameTime) { }
    }
}
