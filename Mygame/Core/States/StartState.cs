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

        public StartState(Mygame.Game1 game)
        {
            _game = game;
        }

        public void LoadContent(ContentManager content)
        {
            _bg = content.Load<Texture2D>("hxh");
            _dst = new Rectangle(0, 0, 1580, 1020);
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                _game.ChangeState(new PlayState(_game, 1));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (_bg != null)
                spriteBatch.Draw(_bg, _dst, Color.White);
            spriteBatch.End();
        }

        public void Unload() { }
    }
}
