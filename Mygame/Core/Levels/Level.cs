using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Mygame.Core.GameLoop;

namespace Mygame.Core.Levels
{
    public class Level
    {
        public int Index { get; }
        public List<IEntity> Entities { get; }
        public Rectangle FinishZone { get; }

        public Level(int index, List<IEntity> entities, Rectangle finishZone)
        {
            Index = index;
            Entities = entities;
            FinishZone = finishZone;
        }
    }
}
