using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerMoveController : IMoveController
    {
        public bool Walks { get; }
        public bool AutoRotate { get; set; }

        public Joystick MoveJoystick { get; }
    }
}
