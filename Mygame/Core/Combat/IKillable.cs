

namespace Mygame.Core.Combat
{
    public interface IKillable
    {
        bool IsDead { get; }
        void Kill();
    }
}
