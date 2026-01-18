using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace Mygame.Core.States
{
    public sealed class StartState:IGameState
    {
        private readonly Mygame.Game1 _game;

        private Texture2D? _bg;
        private Rectangle _dst;

        //START KNOP
        private Rectangle _startButton;
        private Texture2D? _buttonTex;
        private bool _hover;


        public StartState(Mygame.Game1 game)
        {
            _game = game;
        }

        public void LoadContent(ContentManager content)
        {
            _bg = content.Load<Texture2D>("hxh");
            _buttonTex = content.Load<Texture2D>("play3");
            _dst = new Rectangle(0, 0, 1580, 1020);


            _startButton = new Rectangle(
       1580 / 2 - 150,   // x (center)
       1020 / 2 + 200,   // y
       300,              // width
       100               // height
   );
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var mousePos = new Point(mouse.X, mouse.Y);

            _hover = _startButton.Contains(mousePos);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || (_hover && mouse.LeftButton == ButtonState.Pressed))
                _game.ChangeState(new PlayState(_game, 1));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (_bg != null)
                spriteBatch.Draw(_bg, _dst, Color.White);

            if (_buttonTex != null)
            {
                var tint = _hover ? Color.Yellow : Color.White;
                spriteBatch.Draw(_buttonTex, _startButton, tint);
            }

            spriteBatch.End();
        }

        public void Unload() { }
    }
}
