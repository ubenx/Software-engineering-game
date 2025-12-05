using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mygame
{
    internal class BlockSprite : Sprite
    {
        private int width;
        private int height;

        public BlockSprite(Texture2D texture, Vector2 position, int width = 128, int height = 128)
            : base(texture, position)
        {
            this.width = width;
            this.height = height;
        }

        public override Rectangle Rect
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rect, Color.White);
        }
    }
}