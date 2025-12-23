using Microsoft.Xna.Framework.Content;


namespace Mygame.Core.Levels
{
    public interface ILevelFactory
    {
        Level Create(int levelIndex, ContentManager content);
    }
}
