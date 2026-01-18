using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mygame.Core.Rendering
{
    public sealed class AnimatedSpriteRenderer
    {
        //beheert animatie- en render-data voor één sprite
        //één visueel object dat als afbeelding wordt getekend in de gamewereld.

        private readonly Texture2D _texture;

        public Rectangle SourceRectangle { get; private set; }

        public AnimatedSpriteRenderer(Texture2D texture, Rectangle initialSource)
        {
            _texture = texture;
            SourceRectangle = initialSource;
        }

        public void SetSource(Rectangle src) => SourceRectangle = src;

        public void Draw(SpriteBatch sb, Rectangle dst, Color color, SpriteEffects fx = SpriteEffects.None)
        {
            sb.Draw(_texture, dst, SourceRectangle, color, 0f, Vector2.Zero, fx, 0f);
        }
    }
}
