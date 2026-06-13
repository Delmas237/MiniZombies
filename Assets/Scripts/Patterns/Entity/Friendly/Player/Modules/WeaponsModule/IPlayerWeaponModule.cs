using JoystickLib;

namespace PlayerLib
{
    public interface IPlayerWeaponModule : IWeaponModule
    {
        Joystick AttackJoystick { get; }
    }
}
