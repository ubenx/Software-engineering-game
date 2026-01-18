

namespace Mygame.Core.Input
{
    // andere datatype (struct) want dit is enkel puur data zonder logica
    public readonly struct PlayerInput
    {
        public float AxisX { get; }
        public float AxisY { get; }
        public bool ShootPressed { get; }
        public bool JumpPressed { get; }

        public PlayerInput(float axisX, float axisY, bool shootPressed, bool jumpPressed)
        {
            AxisX = axisX;
            AxisY = axisY;
            ShootPressed = shootPressed;
            JumpPressed = jumpPressed;
        }
    }
}
