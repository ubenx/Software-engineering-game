using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Mygame.Core.Input;
using Mygame.Core.GameLoop;
using Mygame.Core.Entities;
using MonoGame.Extended;

namespace Mygame.Core.Levels
{
    public sealed class LevelFactory: ILevelFactory
    {
        public Level Create(int levelIndex, ContentManager content)
        {
            Texture2D blockTex = content.Load<Texture2D>("GonAngry");
            Texture2D playerTex = content.Load<Texture2D>("Walk2");

            var entities = new List<IEntity>();

            // Shared: input service for player
            IInputService input = new KeyboardInputService();

            if (levelIndex == 1)
            {
                var finish = new Rectangle(1400, 900, 120, 120);
                //entities.Add(new BlockEntity(blockTex, new Vector2(400, 200), 128, 128));
                //entities.Add(new BlockEntity(blockTex, new Vector2(700, 300), 128, 128));

                //Players worden gemaakt in PlayState anders breekt collision
                //var player = new PlayerEntity(playerTex, new Vector2(300, 700), input);
                //entities.Add(player);


                return new Level(index: 1,
                entities: entities,
                finishZone: finish,
                mapAsset: "Level1");
            }

            if (levelIndex == 2)
            {
                var finish = new Rectangle(1450, 150, 120, 120);
                //entities.Add(new BlockEntity(blockTex, new Vector2(200, 600), 128, 128));
                //entities.Add(new BlockEntity(blockTex, new Vector2(500, 500), 128, 128));
                //entities.Add(new BlockEntity(blockTex, new Vector2(800, 400), 128, 128));
                //entities.Add(new BlockEntity(blockTex, new Vector2(1100, 300), 128, 128));

                //Players worden gemaakt in PlayState anders breekt collision
                //var player = new PlayerEntity(playerTex, new Vector2(100, 900), input);
                //entities.Add(player);


                return new Level(index: 2,
                entities: entities,
                finishZone: finish,
                mapAsset: "Level2");
            }

            return Create(1, content);
        }
    }
}
