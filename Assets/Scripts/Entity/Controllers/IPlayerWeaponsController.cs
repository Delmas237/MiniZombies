using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerWeaponsController : IWeaponsController
    {
        public Joystick AttackJoystick { get; }
    }
}
