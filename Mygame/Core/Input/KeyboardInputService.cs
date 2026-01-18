using Microsoft.Xna.Framework.Input;


namespace Mygame.Core.Input
{
    public sealed class KeyboardInputService: IInputService
    {
        private KeyboardState _prev;
        public PlayerInput Read()
        {
            var k = Keyboard.GetState();

            float axisX = 0f;
            if (k.IsKeyDown(Keys.A) || k.IsKeyDown(Keys.Left)) axisX -= 1f;
            if (k.IsKeyDown(Keys.D) || k.IsKeyDown(Keys.Right)) axisX += 1f;

            float axisY = 0f;

            bool shootPressed = k.IsKeyDown(Keys.F) && !_prev.IsKeyDown(Keys.F);

            bool jumpNow = k.IsKeyDown(Keys.Up);
            bool jumpPressed = jumpNow && !_prev.IsKeyDown(Keys.Up);

            _prev = k;

            return new PlayerInput(axisX, axisY, shootPressed, jumpPressed);
        }
    }
}
