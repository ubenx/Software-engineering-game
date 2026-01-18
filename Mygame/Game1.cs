using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mygame.Core.States;

namespace Mygame
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IGameState? _currentState;
        private Song? _currentSong;
        private string? _currentSongAsset;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = 1780;
            _graphics.PreferredBackBufferHeight = 1220;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ChangeState(new StartState(this));
        }

        public void ChangeState(IGameState next)
        {
            _currentState?.Unload();
            _currentState = next;
            _currentState.LoadContent(Content);
        }

        public void PlayMusic(ContentManager content, string assetName, float volume = 0.4f, bool loop = true)
        {
            // Als dezelfde muziek al speelt: doe niets
            if (_currentSongAsset == assetName && MediaPlayer.State == MediaState.Playing)
                return;

            _currentSongAsset = assetName;
            _currentSong = content.Load<Song>(assetName);

            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Volume = volume;
            MediaPlayer.Play(_currentSong);
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
                return;
            }

            _currentState?.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _currentState?.Draw(_spriteBatch);
            

            base.Draw(gameTime);
        }
    }
}
