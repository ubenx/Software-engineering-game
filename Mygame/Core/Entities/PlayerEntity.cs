using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mygame.Core.Collision;
using Mygame.Core.GameLoop;
using Mygame.Core.Input;
using Mygame.Core.Rendering;
using Mygame.Core.Collision;


namespace Mygame.Core.Entities
{
    public sealed class PlayerEntity: IEntity, ICollisionAware
    {
        public Vector2 Position { get; set; }
        public ICollider Collider { get; }

        private const float Scale = 0.4f;


        private readonly IInputService _input;
        private CollisionSystem? _collision;

        // rendering/anim
        private readonly AnimatedSpriteRenderer _renderer;
        private SpriteEffects _fx = SpriteEffects.None;

        // anim state similar to your current Player.cs
        private double _secondCounter;
        private int _frameX;
        private const int FrameW = 128;
        private const int FrameH = 128;
        private const int MaxFrameX = 824; // matches your old spritesheet logic

        // movement config (same feel as before)
        private const float Speed = 8f;

        public PlayerEntity(Texture2D playerTexture, Vector2 position, IInputService input)
        {
            Position = position;
            _input = input;

            _renderer = new AnimatedSpriteRenderer(playerTexture, new Rectangle(0, 0, FrameW, FrameH));

            // Zorgt voor grotere karakter
            int scaledW = (int)(FrameW * Scale);
            int scaledH = (int)(FrameH * Scale);

            Collider = new RectCollider(() => Position, new Point(scaledW, scaledH));

        }

        //colision
        public void SetCollision(CollisionSystem collision)
        {
            _collision = collision;
        }

        public void Update(GameTime gameTime)
        {
            var inp = _input.Read();

            bool moving = false;

            float dx = inp.AxisX * Speed;
            float dy = inp.AxisY * Speed;

            if (dx > 0)
            {
                moving = true;
                _fx = SpriteEffects.None;
            }
            else if (dx < 0)
            {
                moving = true;
                _fx = SpriteEffects.FlipHorizontally;
            }

            if (dy != 0)
                moving = true;

            // colision
            if (_collision != null)
            {
                var pos = Position;
                _collision.MoveWithCollision(this, ref pos, dx, dy);
                Position = pos;
            }
            else
            {
                // Fallback (zou normaal niet gebeuren als entity via GameWorld.Add is toegevoegd)
                Position = new Vector2(Position.X + dx, Position.Y + dy);
            }

            // Animation like your old code
            if (moving)
            {
                _secondCounter += gameTime.ElapsedGameTime.TotalSeconds;
                const int fps = 10;
                if (_secondCounter >= 1d / fps)
                {
                    _secondCounter = 0;
                    _frameX += FrameW;

                    if (_frameX > MaxFrameX)
                        _frameX = 0;

                    _renderer.SetSource(new Rectangle(_frameX, 0, FrameW, FrameH));
                }
            }
            else
            {
                _secondCounter = 0;
                _frameX = 0;
                _renderer.SetSource(new Rectangle(0, 0, FrameW, FrameH));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // voor scale en groter manneke
            var dst = new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)(FrameW * Scale),
            (int)(FrameH * Scale)
                );
            _renderer.Draw(spriteBatch, dst, Color.White, _fx);
        }
    }
}
