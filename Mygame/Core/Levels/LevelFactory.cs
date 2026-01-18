using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Mygame.Core.Input;
using Mygame.Core.GameLoop;
using Mygame.Core.Entities;
using MonoGame.Extended;
using Mygame.Core.Entities.Enemy;

namespace Mygame.Core.Levels
{
    public sealed class LevelFactory: ILevelFactory
    {
        public Level Create(int levelIndex, ContentManager content)
        {
            

            var entities = new List<IEntity>();

            // Shared: input service for player
            IInputService input = new KeyboardInputService();

            if (levelIndex == 1)
            {
                var finish = new Rectangle(1400, 900, 120, 120);

                return new Level(index: 1,
                entities: entities,
                finishZone: Rectangle.Empty,
                mapAsset: "Level1",
                backgroundAsset: "BlueBG"
                );
            }

            if (levelIndex == 2)
            {
                var finish = new Rectangle(1450, 150, 120, 120);


                return new Level(index: 2,
                entities: entities,
                finishZone: Rectangle.Empty,
                mapAsset: "Level2",
                backgroundAsset: "BlackBG"
                );
            }
            if (levelIndex == 3)
            {
                var finish = new Rectangle(1450, 150, 120, 120); // pas aan aan jouw map

                return new Level(
                    index: 3,
                    entities: entities,
                    finishZone: Rectangle.Empty,
                    mapAsset: "Level3",
                    patrolEnemyLayerName: "EnemySpawn3",  // <- match met je Tiled layernaam
                    backgroundAsset: "BlueBG"                                     // staticEnemyLayerName kun je laten defaulten of leeg laten, want je gebruikt static niet in level 3
                );
            }


            return Create(1, content);
        }
    }
}
