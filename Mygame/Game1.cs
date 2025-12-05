using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mygame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Player player;


        // add picture to game
        //Texture2D texture;

        // Sprites
        List<Sprite> sprites;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = 1580;
            _graphics.PreferredBackBufferHeight = 1020;

            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            
        }

        protected override void LoadContent()
        {
            sprites = new List<Sprite>();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // add picture to game
            //texture = Content.Load<Texture2D>("hxh");

            //Texture2D texture = Content.Load<Texture2D>("hxh");

            Texture2D gonTexture = Content.Load<Texture2D>("GonAngry");
            Texture2D playerTexture = Content.Load<Texture2D>("Walk2");
            sprites.Add(new BlockSprite(gonTexture, new Vector2(100, 100)));
            sprites.Add(new BlockSprite(gonTexture, new Vector2(400, 200)));
            sprites.Add(new BlockSprite(gonTexture, new Vector2(700, 300)));

            player = new Player(playerTexture, new Vector2(200, 200), sprites);
            sprites.Add(player);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //if(!space_pressed && Keyboard.GetState().IsKeyDown(Keys.Space))
            //{
            //    space_pressed = true;
            //    Debug.WriteLine("Space pressed");
            //}

            //if(Keyboard.GetState().IsKeyUp(Keys.Space))
            //{
            //    space_pressed = false;
            //}

            //List<PersonSprite> killList = new();

            //foreach (var sprite in sprites)
            //{
            //    sprite.Update(gameTime);

            //    if(sprite != player && sprite.Rect.Intersects(player.Rect))
            //    {
            //        killList.Add(sprite);

            //    }
            //}

            //foreach (var sprite in killList)
            //{
            //    sprites.Remove(sprite);
            //}

            foreach (var sprite in sprites)
            {
                sprite.Update(gameTime); 
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // add picture to game
            _spriteBatch.Begin();

            foreach (var sprite in sprites)
            {
                sprite.Draw(_spriteBatch);
            }
 

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
