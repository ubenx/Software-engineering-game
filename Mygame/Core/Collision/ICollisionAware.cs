using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mygame.Core.Collision
{
    internal interface ICollisionAware
    {
        void SetCollision(CollisionSystem collision);
    }
}
