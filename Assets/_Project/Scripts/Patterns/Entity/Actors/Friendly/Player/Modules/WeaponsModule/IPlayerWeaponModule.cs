using Entity;
using JoystickLib;

namespace Player
{
    public interface IPlayerWeaponModule : IEntityWeaponModule
    {
        void PullAutoTrigger();
    }
}
