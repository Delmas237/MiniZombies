using System;
using System.Collections.Generic;
using Weapons;

public interface IWeaponsModule
{
    public event Action<int> BulletsChanged;
    public event Action<Gun> GunChanged;

    public Gun CurrentGun { get; }
    public int Bullets { get; }
    public GunType InitialGun { get; }
    public IReadOnlyList<Gun> Guns { get; }

    public void SetInitialGun();
    public void ChangeGun(GunType gunType);

    public void PullTrigger();

    public void AddBullets(int amount);
    public void SpendBullets(int amount);
}
