using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerMoveModule : IMoveModule
    {
        public bool IsMoving { get; }
        public Joystick MoveJoystick { get; }
        public bool IsTraking { get; }
    }
}
