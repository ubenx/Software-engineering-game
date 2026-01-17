using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mygame.Core.Collision
{
    //klasse die helpt bij zwaartekracht
    public sealed class MoveResult
    {
        public bool HitLeft { get; set; }
        public bool HitRight { get; set; }
        public bool HitTop { get; set; }
        public bool HitBottom { get; set; }
    }
}
