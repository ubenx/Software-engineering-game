using Microsoft.Xna.Framework;


namespace Mygame.Core.Physics
{
    public interface IPhysicsBody
    {
        Vector2 Velocity { get; set; }
        bool IsGrounded { get; set; }
    }
}
