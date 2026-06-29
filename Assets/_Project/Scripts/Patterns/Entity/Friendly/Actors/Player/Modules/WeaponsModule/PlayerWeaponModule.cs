using System;
using Weapons;

namespace Entity.Friendly.Player
{
    [Serializable]
    public class PlayerWeaponModule : EntityWeaponModule, IPlayerWeaponModule
    {
        public void PullAutoTrigger()
        {
            if (!_enabled)
                return;

            if (CurrentGun.FireType == GunFireType.Auto)
                PullTrigger();
        }
    }
}
