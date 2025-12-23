using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mygame.Core.Input
{
    public sealed class KeyboardInputService: IInputService
    {
        public PlayerInput Read()
        {
            var k = Keyboard.GetState();

            float x = 0;
            float y = 0;

            if (k.IsKeyDown(Keys.Left)) x -= 1;
            if (k.IsKeyDown(Keys.Right)) x += 1;

            // Voor nu nog vrije Y-beweging (zoals jij had). Later vervangen door jump+gravity.
            if (k.IsKeyDown(Keys.Up)) y -= 1;
            if (k.IsKeyDown(Keys.Down)) y += 1;

            bool shoot = k.IsKeyDown(Keys.Space);
            bool jump = k.IsKeyDown(Keys.Z);

            return new PlayerInput(x, y, shoot, jump);
        }
    }
}
