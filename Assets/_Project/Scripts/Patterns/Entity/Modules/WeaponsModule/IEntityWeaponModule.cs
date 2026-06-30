using System;
using System.Collections.Generic;
using Weapons;

namespace Entity
{
    public interface IEntityWeaponModule : IModule
    {
        Gun CurrentGun { get; }
        int Bullets { get; }
        GunType InitialGun { get; }
        IReadOnlyList<Gun> Guns { get; }

        event Action<int> BulletsChanged;
        event Action<Gun> GunChanged;

        void SetInitialGun();
        void ChangeGun(GunType gunType);

        void PullTrigger();

        void AddBullets(int amount);
        void SpendBullets(int amount);
    }
}
