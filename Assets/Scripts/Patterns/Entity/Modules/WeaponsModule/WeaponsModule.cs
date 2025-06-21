using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Weapons;

[Serializable]
public class WeaponsModule : IWeaponsModule
{
    [SerializeField] private GunType _initialGun = GunType.Pistol;
    [SerializeField] private int _bullets = 100;
    [SerializeField] private List<Gun> _guns;

    public event Action<int> BulletsChanged;
    public event Action<Gun> GunChanged;

    public Gun CurrentGun { get; private set; }
    
    public int Bullets => _bullets;
    public GunType InitialGun => _initialGun;
    public IReadOnlyList<Gun> Guns => _guns;

    public virtual void Initialize()
    {
        SetInitialGun();
    }
    public void SetInitialGun() => ChangeGun(_initialGun);

    public void PullTrigger()
    {
        if (Bullets - CurrentGun.Consumption >= 0)
        {
            if (CurrentGun.ShootRequest())
                SpendBullets(CurrentGun.Consumption);
        }
    }

    public virtual void ChangeGun(GunType gunType)
    {
        if (_guns.Any(g => g.Type == gunType))
        {
            CurrentGun = _guns.First(g => g.Type == gunType);
            GunChanged?.Invoke(CurrentGun);
            UpdateGunsVisible();
        }
        else
            throw new NullReferenceException($"No weapon with type {gunType} found");
    }

    private void UpdateGunsVisible()
    {
        for (int i = 0; i < Guns.Count; i++)
            _guns[i].gameObject.SetActive(CurrentGun.Type == _guns[i].Type);
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
