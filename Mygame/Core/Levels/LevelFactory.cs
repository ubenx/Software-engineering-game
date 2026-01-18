using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Mygame.Core.Input;
using Mygame.Core.GameLoop;


namespace Mygame.Core.Levels
{
    public sealed class LevelFactory: ILevelFactory
    {
        public Level Create(int levelIndex, ContentManager content)
        {
            

            var entities = new List<IEntity>();

            // gedeelt: input service voor spelers
            IInputService input = new KeyboardInputService();

            if (levelIndex == 1)
            {
                return new Level(index: 1,
                entities: entities,
                finishZone: Rectangle.Empty,
                mapAsset: "Level1",
                backgroundAsset: "BlueBG"
                );
            }

            if (levelIndex == 2)
            {
                return new Level(index: 2,
                entities: entities,
                finishZone: Rectangle.Empty,
                mapAsset: "Level2",
                backgroundAsset: "BlackBG"
                );
            }
            if (levelIndex == 3)
            {
                return new Level(
                    index: 3,
                    entities: entities,
                    finishZone: Rectangle.Empty,
                    mapAsset: "Level3",
                    patrolEnemyLayerName: "EnemySpawn3",  // Tiled layernaam
                    backgroundAsset: "BlueBG"                                     // staticEnemyLayerName kun je laten defaulten of leeg laten, want je gebruikt static niet in level 3
                );
            }


            return Create(1, content);
        }
    }
}
