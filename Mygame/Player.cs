using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mygame
{
    internal class Player : PersonSprite
    {

        List<Sprite> collisionGroup;

        //variabele voor animatie
        private int counter;
        private double secondCounter;
        private int schuifOp_X = 0;

        public Player(Texture2D texture, Vector2 position, List<Sprite> collisionGroup) : base(texture, position)
        {
            this.collisionGroup = collisionGroup;
        }


        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float changeX = 0;

            bool beweegt = false;


            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                changeX += 5;
                //voor animatie
                beweegt = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                changeX -= 5;
                //voor animatie
                beweegt = true;
            }
            position.X += changeX;

            foreach (var sprite in collisionGroup)
            {

                if (sprite != this && sprite.Rect.Intersects(Rect))
                {
                    position.X -= changeX;
                }
            }

            float changeY = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                changeY -= 5;
                //voor animatie
                beweegt = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                changeY += 5;
                //voor animatie
                beweegt = true;
            }
            position.Y += changeY;

            foreach (var sprite in collisionGroup)
            {

                if (sprite != this && sprite.Rect.Intersects(Rect))
                {
                    position.Y -= changeY;
                }
            }

            // animatie specifiek voor 1 soort sprite
            if (beweegt)
            {
                secondCounter += gameTime.ElapsedGameTime.TotalSeconds;
                int fps = 10;
                if (secondCounter >= 1d / fps)
                {
                    counter++;
                    secondCounter = 0;
                    schuifOp_X += 128;
                    if (schuifOp_X > 824)
                    {
                        schuifOp_X = 0;
                    }
                    SourceRectangle = new Rectangle(schuifOp_X, 0, 128, 128);
                }
            }
            else
            {
                SourceRectangle = new Rectangle(0, 0, 128, 128);
                schuifOp_X = 0;
                counter = 0;
                secondCounter = 0;
            }


        }
    }
}
