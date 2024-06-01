using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

[Serializable]
public class WeaponsController : IWeaponsController
{
    [SerializeField] private int _bullets = 100;
    public int Bullets => _bullets;
    public event Action<int> BulletsChanged;

    [SerializeField] private List<Gun> _guns;
    public IReadOnlyList<Gun> Guns => _guns;

    public Gun CurrentGun { get; private set; }
    public event Action<Gun> GunChanged;

    public virtual void Initialize()
    {
        ChangeGun(GunType.Pistol);
    }

    public void PullTrigger()
    {
        if (Bullets - CurrentGun.Consumption >= 0)
        {
            if (CurrentGun.ShootRequest())
                SpendBullets(CurrentGun.Consumption);
        }
    }
    public void PullAutoTrigger()
    {
        if (Bullets - CurrentGun.Consumption >= 0
            && CurrentGun.FireType == GunFireType.Auto)
        {
            if (CurrentGun.ShootRequest())
                SpendBullets(CurrentGun.Consumption);
        }
    }

    public virtual void ChangeGun(GunType gunType)
    {
        CurrentGun = _guns[(int)gunType];
        GunChanged?.Invoke(CurrentGun);

        UpdateGunsVisible();
    }

    private void UpdateGunsVisible()
    {
        int currentGunIndex = (int)CurrentGun.Type;

        for (int i = 0; i < Guns.Count; i++)
            _guns[i].gameObject.SetActive(i == currentGunIndex);
    }

    public void AddBullets(int amount)
    {
        if (amount <= 0)
            return;

        _bullets += amount;
        BulletsChanged?.Invoke(_bullets);
    }

    public void SpendBullets(int amount)
    {
        if (amount <= 0)
            return;

        _bullets -= amount;
        BulletsChanged?.Invoke(_bullets);
    }
}
