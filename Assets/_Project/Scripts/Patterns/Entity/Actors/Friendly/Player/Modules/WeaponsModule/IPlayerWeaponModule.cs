using Entity;
using JoystickLib;

namespace Player
{
    public interface IPlayerWeaponModule : IEntityWeaponModule
    {
        Joystick AttackJoystick { get; }
    }
}
