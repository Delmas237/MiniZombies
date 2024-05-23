using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerMoveController : IMoveController
    {
        public bool Walks { get; }
        public Joystick MoveJoystick { get; }
    }
}
