using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mygame.Core.Collision;
using Mygame.Core.GameLoop;
using Mygame.Core.Input;
using Mygame.Core.Rendering;
using Mygame.Core.Physics;
using System;


namespace Mygame.Core.Entities.Player
{
    public sealed class PlayerEntity : IEntity, IPhysicsBody
    {
        public Vector2 Position { get; set; }
        public ICollider Collider { get; }

        public Vector2 Velocity { get; set; }  
        public bool IsGrounded { get; set; }

        public int Lives { get; private set; } = 3;


        // Voor leven systeem
        public bool IsInvulnerable => _iFrameTimer > 0f;
        //visueale markering
        private double _blinkTime;

        private float _iFrameTimer = 0f;
        private const float IFrameDuration = 1.0f; // 1 sec


        //buffers voor het springen  anders werkt het te sluggish
        private float _jumpBufferTimer = 0f;
        private const float JumpBufferTime = 0.12f; // 120ms

        private float _coyoteTimer = 0f;
        private const float CoyoteTime = 0.10f; // 100ms

        private const float Scale = 0.4f;
        float moveSpeed = 250f;

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
        private const int MaxFrameX = 824; 

        // movement config 
        
        private const float JumpVelocity = -400f; // px/sec omhoog (negatief)
        


        

        public PlayerEntity(Texture2D playerTexture, Vector2 position, IInputService input, CollisionSystem collision)
        {
            Position = position;
            _input = input;
            _collision = collision;

            _renderer = new AnimatedSpriteRenderer(playerTexture, new Rectangle(0, 0, FrameW, FrameH));

            int scaledW = (int)(FrameW * Scale);
            int scaledH = (int)(FrameH * Scale);

     


            int hitW = (int)(scaledW * 0.40f);   //  smaller maken
            int hitH = (int)(scaledH * 0.70f);

            int offX = (scaledW - hitW) / 2;     // centreren 
            int offY = (scaledH - hitH);



            Collider = new RectCollider(() => Position, new Point(hitW, hitH), new Point(offX, offY));
        }

        

        public void Update(GameTime gameTime)
        {
            var inp = _input.Read();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //ook voor levens
            if (_iFrameTimer > 0f)
                _iFrameTimer = MathF.Max(0f, _iFrameTimer - dt);
            _blinkTime += gameTime.ElapsedGameTime.TotalSeconds;


            // buffer: als je drukt, onthouden we het even
            if (inp.JumpPressed)
                _jumpBufferTimer = JumpBufferTime;
            else
                _jumpBufferTimer = MathF.Max(0f, _jumpBufferTimer - dt);

            // coyote: als je op grond staat, reset timer, anders loopt hij af
            if (IsGrounded)
                _coyoteTimer = CoyoteTime;
            else
                _coyoteTimer = MathF.Max(0f, _coyoteTimer - dt);



            

             // pixels/sec (pas aan)
            Velocity = new Vector2(inp.AxisX * moveSpeed, Velocity.Y);

            
            // jump als:
            // - je onlangs jump hebt gedrukt 
            // - en je bent grounded OF net pas van grond gevallen 
            if (_jumpBufferTimer > 0f && _coyoteTimer > 0f)
            {
                Velocity = new Vector2(Velocity.X, JumpVelocity);
                IsGrounded = false;
                _jumpBufferTimer = 0f;
                _coyoteTimer = 0f;
            }



            bool moving = inp.AxisX != 0;

            if (inp.AxisX > 0) _fx = SpriteEffects.None;
            else if (inp.AxisX < 0) _fx = SpriteEffects.FlipHorizontally;


            UpdateAnimation(gameTime, moving);
        }

        private void UpdateAnimation(GameTime gameTime, bool moving)
        {
            if (!moving)
            {
                _secondCounter = 0;
                _frameX = 0;
                _renderer.SetSource(new Rectangle(0, 0, FrameW, FrameH));
                return;
            }

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

        public bool TryTakeHit()
        {
            if (IsInvulnerable) return false;

            Lives--;
            _iFrameTimer = IFrameDuration;
            return true;
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsInvulnerable)
            {
                // 10x per seconde aan/uit
                bool visible = ((int)(_blinkTime * 10) % 2) == 0;
                if (!visible) return;
            }

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
