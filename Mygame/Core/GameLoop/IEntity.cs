using Microsoft.Xna.Framework;
using Mygame.Core.Collision;

namespace Mygame.Core.GameLoop
{
    public interface IEntity: IUpdatable, IRenderable
    {
        Vector2 Position { get; set; }
        ICollider? Collider { get; }
    }
}
