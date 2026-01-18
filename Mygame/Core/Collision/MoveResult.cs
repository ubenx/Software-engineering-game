

namespace Mygame.Core.Collision
{

    //WELKE KANTEN van je entity collision hebben geraakt tijdens beweging.
    public sealed class MoveResult
    {
        public bool HitLeft { get; set; }
        public bool HitRight { get; set; }
        public bool HitTop { get; set; }
        public bool HitBottom { get; set; }
    }
}
