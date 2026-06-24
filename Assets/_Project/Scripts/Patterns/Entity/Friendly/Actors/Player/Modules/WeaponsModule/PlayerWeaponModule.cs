using Entity;
using System;
using Weapons;

namespace Player
{
    [Serializable]
    public class PlayerWeaponModule : EntityWeaponModule, IPlayerWeaponModule
    {
        public void PullAutoTrigger()
        {
            if (CurrentGun.FireType == GunFireType.Auto)
                PullTrigger();
        }
    }
}
