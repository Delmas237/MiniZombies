using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerWeaponsModule : IWeaponsModule
    {
        Joystick AttackJoystick { get; }
    }
}
