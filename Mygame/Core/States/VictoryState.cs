using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Mygame.Core.States
{
    internal class VictoryState : IGameState
    {
        private readonly Mygame.Game1 _game;

        private Texture2D? _tex;
        private Rectangle _dst;

        public VictoryState(Mygame.Game1 game)
        {
            _game = game;
        }

        public void LoadContent(ContentManager content)
        {
            _tex = content.Load<Texture2D>("Victory");
            _dst = new Rectangle(0, 0, 1780, 1220);
            MediaPlayer.Stop();
        }

        public void Update(GameTime gameTime)
        {
            var k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Enter))
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
