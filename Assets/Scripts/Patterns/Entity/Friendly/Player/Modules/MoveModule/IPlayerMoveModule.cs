using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerMoveModule : IMoveModule
    {
        bool IsMoving { get; }
        Joystick MoveJoystick { get; }
        bool IsTraking { get; }
    }
}
