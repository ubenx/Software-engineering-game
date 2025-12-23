using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Mygame.Core.GameLoop;
using Mygame.Core.Levels;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using Mygame.Core.Entities;
using Autofac.Features.Metadata;
using MonoGame.Extended.ECS.Systems;
using Mygame.Core.Input;




namespace Mygame.Core.States
{
    public sealed class PlayState: IGameState
    {
        private readonly Mygame.Game1 _game;
        private readonly int _levelIndex;

        private readonly ILevelFactory _factory = new LevelFactory();

        private Level? _level;
        private GameWorld? _world;

        // Tiled map tekenen
        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;
        private OrthographicCamera _camera;


        // for drawing finish zone
        private Texture2D? _whitePixel;

        public PlayState(Mygame.Game1 game, int levelIndex)
        {
            _game = game;
            _levelIndex = levelIndex;
        }

        public void LoadContent(ContentManager content)
        {
            _level = _factory.Create(_levelIndex, content);

            _world = new GameWorld();
            _world.AddRange(_level.Entities);



            _whitePixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            _whitePixel.SetData(new[] { Color.White });

            _map = content.Load<TiledMap>("Level1");
            _mapRenderer = new TiledMapRenderer(_game.GraphicsDevice, _map);

            

            // COLLISIONS uit Tiled
            var colLayer = _map.ObjectLayers.FirstOrDefault(l => l.Name == "Collisions");

            if (colLayer != null)
            {
                foreach (var obj in colLayer.Objects)
                {
                    // Tiled object rectangles
                    var rect = new Rectangle(
                        (int)obj.Position.X,
                        (int)obj.Position.Y,
                        (int)obj.Size.Width,
                        (int)obj.Size.Height
                    );

                    _world.Add(new CollisionBlockEntity(rect));
                }
            }


            // 2) Player maken met collision-system
            IInputService input = new KeyboardInputService();
            var playerTex = content.Load<Texture2D>("Walk2");

            var player = new PlayerEntity(playerTex, Vector2.Zero, input, _world.Collision);
            _world.Add(player);

            // 3) Spawn uit Tiled toepassen
            //var spawnLayer = _map.ObjectLayers.FirstOrDefault(l => l.Name == "PlayerSpawn");
            //if (spawnLayer != null)
            //{
            //    var spawn = spawnLayer.Objects.FirstOrDefault(); // of o => o.Name == "PlayerSpawn"
            //    if (spawn != null)
            //        player.Position = spawn.Position;
            //}



            // spawn
            var spawnLayer = _map.ObjectLayers.FirstOrDefault(l => l.Name == "PlayerSpawn");

            if (spawnLayer != null)
            {
                var spawn = spawnLayer.Objects.FirstOrDefault();

                if (spawn != null)
                {
                    var player1 = _world.FindFirst<PlayerEntity>();
                    if (player != null)
                    {
                        player.Position = spawn.Position;
                    }
                }
            }


            // 🔽 camera
            _camera = new OrthographicCamera(_game.GraphicsDevice);
            _camera.Zoom = 3f;
        }

        public void Update(GameTime gameTime)
        {
            if (_level == null || _world == null) return;

            _world.Update(gameTime);


            // update map renderer (anim tiles, etc.)
            _mapRenderer.Update(gameTime);

            // camera laten volgen
            var player = _world.FindFirst<PlayerEntity>();
            if (player != null)
                _camera.LookAt(player.Position);

            if (player != null)
            {
                // Win condition
                if (player.Collider != null && player.Collider.Bounds.Intersects(_level.FinishZone))
                {
                    int next = _level.Index + 1;
                    if (next > 2)
                        _game.ChangeState(new StartState(_game));
                    else
                        _game.ChangeState(new PlayState(_game, next));
                    return;
                }

                // Death placeholder
                if (player.Position.Y > 1200)
                {
                    _game.ChangeState(new GameOverState(_game));
                    return;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_level == null || _world == null) return;

            // 1️⃣ Tiled map (achtergrond)
            _mapRenderer.Draw(_camera.GetViewMatrix());

            // 2️⃣ Entities (player, blocks, enemies)
            spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            _world.Draw(spriteBatch);
            

            // Draw finish zone visibly
            if (_whitePixel != null)
                spriteBatch.Draw(_whitePixel, _level.FinishZone, Color.White * 0.35f);

            spriteBatch.End();
        }

        public void Unload()
        {
            _whitePixel?.Dispose();
            _whitePixel = null;
        }
    }
}
