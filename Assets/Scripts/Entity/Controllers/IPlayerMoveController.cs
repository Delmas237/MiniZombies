using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerMoveController : IMoveController
    {
        public bool IsMoving { get; }
        public Joystick MoveJoystick { get; }
    }
}
