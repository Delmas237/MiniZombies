using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerMovementModule : IMovementModule
    {
        bool IsMoving { get; }
        Joystick MoveJoystick { get; }
        bool IsTraking { get; }
    }
}
