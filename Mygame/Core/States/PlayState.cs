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
using Mygame.Core.Input;
using Mygame.Core.Entities.Player;
using Mygame.Core.Entities.Enemy;
using System;





namespace Mygame.Core.States
{
    public sealed class PlayState: IGameState
    {
        private readonly Mygame.Game1 _game;
        private Texture2D? _background;
        private readonly int _levelIndex;
        private readonly Mygame.Core.Combat.DamageSystem _damage = new();
        private readonly Mygame.Core.Combat.StompSystem _stomp = new();


        //textuur hart
        private Texture2D? _heartTex;


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

            _background = content.Load<Texture2D>(_level.BackgroundAsset);
            _heartTex = content.Load<Texture2D>("heart");


            _world = new GameWorld();
            _world.AddRange(_level.Entities);


            _game.PlayMusic(content, "pixelAudio", volume: 0.4f, loop: true);



            _whitePixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            _whitePixel.SetData(new[] { Color.White });

            _map = content.Load<TiledMap>(_level.MapAsset);
            _mapRenderer = new TiledMapRenderer(_game.GraphicsDevice, _map);

            // FINISH uit Tiled
            var finishLayer = _map.ObjectLayers.FirstOrDefault(l => l.Name == _level.FinishLayerName);
            var finishObj = finishLayer?.Objects.FirstOrDefault();

            if (finishObj != null)
            {
                _level.FinishZone = new Rectangle(
                    (int)finishObj.Position.X,
                    (int)finishObj.Position.Y,
                    (int)finishObj.Size.Width,
                    (int)finishObj.Size.Height
                );
            }
            else
            {
                // fallback voor debug
                _level.FinishZone = Rectangle.Empty;
            }


            // COLLISIONS uit Tiled
            
            var colLayer = _map.ObjectLayers.FirstOrDefault(l => l.Name == _level.CollisionLayerName);


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


            // Player maken met collision-system
            IInputService input = new KeyboardInputService();
            var playerTex = content.Load<Texture2D>("Walk2");

            var player = new PlayerEntity(playerTex, Vector2.Zero, input, _world.Collision);
            _world.Add(player);



            // Playerspawn
            
            var spawnLayer = _map.ObjectLayers.FirstOrDefault(l => l.Name == _level.PlayerSpawnLayerName);
            var spawn = spawnLayer?.Objects.FirstOrDefault();
            var player1 = _world.FindFirst<PlayerEntity>();

            if (spawn != null && player1?.Collider is Mygame.Core.Collision.RectCollider rc)
            {
                // Plaats speler zodat zijn COLLIDER binnen het spawn-rect valt

                float spawnLeft = spawn.Position.X;
                float spawnTop = spawn.Position.Y;
                float spawnW = spawn.Size.Width > 0 ? spawn.Size.Width : 0;
                float spawnH = spawn.Size.Height > 0 ? spawn.Size.Height : 0;

                // center X in spawn rect
                float colliderCenterX = rc.Offset.X + rc.Size.X / 2f;
                float targetCenterX = spawnLeft + spawnW / 2f;

                float x = targetCenterX - colliderCenterX;

                // bottom align: collider bottom op spawn bottom (met 2px marge omhoog)
                float colliderBottom = rc.Offset.Y + rc.Size.Y;
                float targetBottom = spawnTop + spawnH;

                float y = (targetBottom - colliderBottom) - 2f; // -2px zodat je niet in vloer start

                player1.Position = new Vector2(x, y);
            }



            //  Static Enemy spawn uit Tiled
            var enemySpawnLayer = _map.ObjectLayers.FirstOrDefault(l => l.Name == _level.StaticEnemyLayerName);

            if (enemySpawnLayer != null)
            {
                Texture2D staticEnemyTex = content.Load<Texture2D>("staticEnemy1");

                foreach (var obj in enemySpawnLayer.Objects)
                {
                    int w = (int)(obj.Size.Width > 0 ? obj.Size.Width : 64);
                    int h = (int)(obj.Size.Height > 0 ? obj.Size.Height : 64);

                    var rect = new Rectangle(
                        (int)obj.Position.X,
                        (int)obj.Position.Y,
                        w,
                        h
                    );

                    _world.Add(new StaticEnemyEntity(staticEnemyTex, rect));
                }
            }

            //  Patrol Enemy spawn uit Tiled
            var enemySpawnLayer2 = _map.ObjectLayers.FirstOrDefault(l => l.Name == _level.PatrolEnemyLayerName);

            if (enemySpawnLayer2 != null)
            {
                Texture2D patrolEnemyTex =
                    _level.Index == 3
                        ? content.Load<Texture2D>("movingEnemy2") // nieuw texture voor level 3
                        : content.Load<Texture2D>("movingEnemy1");


                foreach (var obj in enemySpawnLayer2.Objects)
                {
                    int w = (int)(obj.Size.Width > 0 ? obj.Size.Width : 64);
                    int h = (int)(obj.Size.Height > 0 ? obj.Size.Height : 64);

                    var rect = new Rectangle(
                        (int)obj.Position.X,
                        (int)obj.Position.Y,
                        w,
                        h
                    );

                    float speed = _level.Index == 3 ? 240f : 140f;
                    _world.Add(new PatrolEnemyEntity(patrolEnemyTex, rect, speed));

                }
            }



            //  camera instelling
            _camera = new OrthographicCamera(_game.GraphicsDevice);
            _camera.Zoom = 3f;

           

        }

        public void Update(GameTime gameTime)
        {
            if (_level == null || _world == null) return;
            var player = _world.FindFirst<PlayerEntity>();



            _world.Update(gameTime);

            // Stomp mechanisme (MOET hier gebeuren)
            bool stomped = false;

            if (player != null)
            {
                stomped = _stomp.TryStomp(_world, player);

                // Alleen damage als je NIET gestompt hebt
                if (!stomped && _damage.CheckPlayerHit(_world, player))
                {
                    if (player.TryTakeHit())
                    {
                        var enemy1 = _world.FindAll<IEntity>()
                           .FirstOrDefault(e =>
                               e is IDamaging &&
                               e.Collider != null &&
                               player.Collider != null &&
                               e.Collider.Bounds.Intersects(player.Collider.Bounds));

                        if (enemy1?.Collider != null)
                        {
                            float dir = MathF.Sign(
                                player.Position.X - enemy1.Collider.Bounds.Center.X
                            );
                            player.Velocity = new Vector2(dir * 650f, -340f);
                        }

                        if (player.Lives <= 0)
                        {
                            _game.ChangeState(new GameOverState(_game));
                            return;
                        }
                    }
                }
            }







            var enemy = _world.FindFirst<Mygame.Core.Entities.Enemy.StaticEnemyEntity>();

            if (player?.Collider != null && enemy?.Collider != null)
            {
                var p = player.Collider.Bounds;
                var e = enemy.Collider.Bounds;

                System.Diagnostics.Debug.WriteLine($"PlayerBounds={p} EnemyBounds={e} Intersects={p.Intersects(e)}");
                System.Diagnostics.Debug.WriteLine($"gapX: {e.Left - p.Right}, gapY: {e.Top - p.Bottom}");
            }




            // update map renderer (anim tiles, etc.)
            _mapRenderer.Update(gameTime);

            // camera laten volgen
            
            if (player != null)
                _camera.LookAt(player.Position);

            if (player != null)
            {
                // Win condition-> Laatste level beïndigen

                if (_level.FinishZone != Rectangle.Empty &&
                    player.Collider != null &&
                    player.Collider.Bounds.Intersects(_level.FinishZone))
                {
                    int next = _level.Index + 1;
                    if (next > 3)
                        _game.ChangeState(new VictoryState(_game));
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

            //  Achtergrond (Volgt mee)
            if (_background != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(
                    _background,
                    new Rectangle(
                        0, 0,
                        _game.GraphicsDevice.Viewport.Width,
                        _game.GraphicsDevice.Viewport.Height),
                    Color.White
                );
                spriteBatch.End();
            }

            //  Tiled map (world-space)
            _mapRenderer.Draw(_camera.GetViewMatrix());

            



            // Entities + debug (world-space)
            spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());

            _world.Draw(spriteBatch);


            //Om collision zichtbaar te maken, tijdens debuggen
            if (_whitePixel != null)
            {
                foreach (var e in _world.FindAll<IEntity>())
                {
                    if (e.Collider == null) continue;
                    var r = e.Collider.Bounds;
                    spriteBatch.Draw(_whitePixel, r, Color.Red * 0.35f);
                }
            }

            spriteBatch.End();

            //Visuele voorstelling van levens

            // HUD (screen-space)
            var player = _world.FindFirst<PlayerEntity>();

            if (player != null && _heartTex != null)
            {
                spriteBatch.Begin();

                int size = 50;   // grootte van hartje (pas aan)
                int pad = 6;     // ruimte tussen hartjes
                int x0 = 12;
                int y0 = 12;

                for (int i = 0; i < player.Lives; i++)
                {
                    spriteBatch.Draw(
                        _heartTex,
                        new Rectangle(
                            x0 + i * (size + pad),
                            y0,
                            size,
                            size),
                        Color.White
                    );
                }

                spriteBatch.End();
            }

        }


        public void Unload()
        {
            _whitePixel?.Dispose();
            _whitePixel = null;
        }
    }
}
