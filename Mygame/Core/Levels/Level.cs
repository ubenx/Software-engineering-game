using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Mygame.Core.GameLoop;


namespace Mygame.Core.Levels
{
    public class Level
    {
        public int Index { get; }
        public List<IEntity> Entities { get; }
        public Rectangle FinishZone { get; set; }
        public string FinishLayerName { get; }


        public string MapAsset { get; }
        public string CollisionLayerName { get; }
        public string PlayerSpawnLayerName { get; }

        public string StaticEnemyLayerName { get; set; }

        public string PatrolEnemyLayerName { get; set; }
        public string BackgroundAsset { get; }


        public Level(
                int index,
                List<IEntity> entities,
                Rectangle finishZone,
                string mapAsset,
                string collisionLayerName = "Collisions",
                string playerSpawnLayerName = "PlayerSpawn",
                string staticEnemyLayerName = "EnemySpawn1",
                string patrolEnemyLayerName = "EnemySpawn2",
                string finishLayerName = "Finish",
                string backgroundAsset = "BlueBG"
            )
                    {
                        Index = index;
                        Entities = entities;
                        FinishZone = finishZone;
                        MapAsset = mapAsset;

                        CollisionLayerName = collisionLayerName;
                        PlayerSpawnLayerName = playerSpawnLayerName;
                        StaticEnemyLayerName = staticEnemyLayerName;
                        PatrolEnemyLayerName = patrolEnemyLayerName;
                        FinishLayerName = finishLayerName;

                        BackgroundAsset = backgroundAsset;
        }

    }
}
