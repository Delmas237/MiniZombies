using System;
using System.Collections.Generic;
using Weapons;

namespace Entity
{
    public interface IEntityWeaponModule
    {
        event Action<int> BulletsChanged;
        event Action<Gun> GunChanged;

        Gun CurrentGun { get; }
        int Bullets { get; }
        GunType InitialGun { get; }
        IReadOnlyList<Gun> Guns { get; }

        void SetInitialGun();
        void ChangeGun(GunType gunType);

        void PullTrigger();

        void AddBullets(int amount);
        void SpendBullets(int amount);
    }
}
