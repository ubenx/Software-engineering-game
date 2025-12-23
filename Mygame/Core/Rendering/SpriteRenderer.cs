using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mygame.Core.Rendering
{
    public sealed class SpriteRenderer
    {
            private readonly Texture2D _texture;

            public SpriteRenderer(Texture2D texture)
            {
                _texture = texture;
            }

            public void Draw(SpriteBatch sb, Rectangle dst, Color color, SpriteEffects fx = SpriteEffects.None)
            {
                sb.Draw(_texture, dst, null, color, 0f, Vector2.Zero, fx, 0f);
            }
        
    }
}
