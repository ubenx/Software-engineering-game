using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mygame.Core.Collision;
using Mygame.Core.GameLoop;
using Mygame.Core.Input;
using Mygame.Core.Rendering;
using Mygame.Core.Collision;


namespace Mygame.Core.Entities
{
    public sealed class PlayerEntity: IEntity
    {
        public Vector2 Position { get; set; }
        public ICollider Collider { get; }

        public Vector2 Velocity { get; set; }   // nieuw
        public bool IsGrounded { get; set; }    // nieuw (optioneel)


        private const float Scale = 0.4f;


        private readonly IInputService _input;
        private readonly CollisionSystem _collision;

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


        public PlayerEntity(Texture2D playerTexture, Vector2 position, IInputService input, CollisionSystem collision)
        {
            Position = position;
            _input = input;
            _collision = collision;

            _renderer = new AnimatedSpriteRenderer(playerTexture, new Rectangle(0, 0, FrameW, FrameH));

            int scaledW = (int)(FrameW * Scale);
            int scaledH = (int)(FrameH * Scale);

            // hitbox kleiner maken
            //int hitW = scaledW - 20;   // ← pas dit getal aan
            //int hitH = scaledH - 3;    // ← optioneel

            int hitW = (int)(scaledW * 0.35f);
            int hitH = (int)(scaledH * 0.85f);

            // hitbox centreren binnen sprite
            int offX = (int)(scaledW * 0.25f);   // Was 0.32f → nu meer naar links
            int offY = 0;



            Collider = new RectCollider(() => Position, new Point(hitW, hitH), new Point(offX, offY));
        }


        //colision
        //public void SetCollision(CollisionSystem collision)
        //{
        //    _collision = collision;
        //}

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
            var pos = Position;
            _collision.MoveWithCollision(this, ref pos, dx, dy);
            Position = pos;


            //else
            //{
            //    // Fallback (zou normaal niet gebeuren als entity via GameWorld.Add is toegevoegd)
            //    Position = new Vector2(Position.X + dx, Position.Y + dy);
            //}

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
