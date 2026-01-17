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

        public string MapAsset { get; }
        public string CollisionLayerName { get; }
        public string PlayerSpawnLayerName { get; }

        public Level(int index, List<IEntity> entities, Rectangle finishZone, string mapAsset,
            string collisionLayerName = "Collisions",
            string playerSpawnLayerName = "PlayerSpawn")
        {
            Index = index;
            Entities = entities;
            FinishZone = finishZone;

            MapAsset = mapAsset;
            CollisionLayerName = collisionLayerName;
            PlayerSpawnLayerName = playerSpawnLayerName;
        }
    }
}
