using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mygame
{
    internal class PersonSprite : Sprite
    {
        public Rectangle SourceRectangle { get; set; } = new Rectangle(0, 0, 128, 128);

        public PersonSprite(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
        }

        // Override Rect zodat deze overeenkomt met het frame
        public override Rectangle Rect
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, SourceRectangle.Width, SourceRectangle.Height);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
{
    spriteBatch.Draw(texture, Rect, SourceRectangle, Color.White);
}
    }
}