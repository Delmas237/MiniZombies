using System;
using System.Collections.Generic;
using Weapons;

public interface IWeaponsController
{
    public int Bullets { get; }
    public event Action<int> BulletsChanged;

    public IReadOnlyList<Gun> Guns { get; }
    public Gun CurrentGun { get; }
    public event Action<Gun> GunChanged;
    public void ChangeGun(GunType gunType);

    public void PullTrigger();
    public void PullAutoTrigger();

    public void AddBullets(int amount);
    public void SpendBullets(int amount);
}
