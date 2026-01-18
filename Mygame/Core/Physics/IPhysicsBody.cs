using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mygame.Core.Physics
{
    public interface IPhysicsBody
    {
        Vector2 Velocity { get; set; }
        bool IsGrounded { get; set; }
    }
}
