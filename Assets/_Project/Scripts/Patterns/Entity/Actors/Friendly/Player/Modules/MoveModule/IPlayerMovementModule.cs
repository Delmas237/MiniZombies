using Entity;
using JoystickLib;

namespace Player
{
    public interface IPlayerMovementModule : IEntityMovementModule
    {
        bool IsMoving { get; }
        Joystick MoveJoystick { get; }
        bool IsTraking { get; }
    }
}
