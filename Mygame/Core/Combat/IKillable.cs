using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mygame.Core.Combat
{
    public interface IKillable
    {
        bool IsDead { get; }
        void Kill();
    }
}
