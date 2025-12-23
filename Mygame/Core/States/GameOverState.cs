using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mygame.Core.States
{
    public class GameOverState: IGameState
    {
        private readonly Mygame.Game1 _game;

        private Texture2D? _tex;
        private Rectangle _dst;

        public GameOverState(Mygame.Game1 game)
        {
            _game = game;
        }

        public void LoadContent(ContentManager content)
        {
            _tex = content.Load<Texture2D>("GonAngry");
            _dst = new Rectangle(0, 0, 1580, 1020);
        }

        public void Update(GameTime gameTime)
        {
            var k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.R))
                _game.ChangeState(new PlayState(_game, 1));

            if (k.IsKeyDown(Keys.M))
                _game.ChangeState(new StartState(_game));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (_tex != null)
                spriteBatch.Draw(_tex, _dst, Color.White);
            spriteBatch.End();

        }

        public void Unload() { }
    }
}
