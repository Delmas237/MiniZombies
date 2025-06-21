using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerWeaponsModule : IWeaponsModule
    {
        public Joystick AttackJoystick { get; }
    }
}
