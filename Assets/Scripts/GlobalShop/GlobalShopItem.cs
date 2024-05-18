using System;
using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    [Serializable]
    public class GlobalShopItem
    {
        [SerializeField] private GunType _gunType;
        private GunData _gunData;

        [SerializeField] private GlobalShopParameter _damageParameter;
        [SerializeField] private GlobalShopParameter _cooldownParameter;
        [SerializeField] private GlobalShopParameter _distanceParameter;

        public event Action Updated;

        public void Intialize()
        {
            _gunData = GunsManager.Guns[(int)_gunType];
            UpdateInfo();

            _damageParameter.Initialize();
            _cooldownParameter.Initialize();
            _distanceParameter.Initialize();

            _damageParameter.Purchased += UpgradeDamage;
            _cooldownParameter.Purchased += UpgradeCooldown;
            _distanceParameter.Purchased += UpgradeDistance;
        }
        public void OnDestroy()
        {
            _damageParameter.Purchased -= UpgradeDamage;
            _cooldownParameter.Purchased -= UpgradeCooldown;
            _distanceParameter.Purchased -= UpgradeDistance;
        }

        private void UpdateInfo()
        {
            _damageParameter.Info.Value = _gunData.Damage;
            _cooldownParameter.Info.Value = _gunData.Cooldown;
            _distanceParameter.Info.Value = _gunData.Distance;

            _damageParameter.UpdateText();
            _cooldownParameter.UpdateText();
            _distanceParameter.UpdateText();
        }

        private void UpgradeDamage()
        {
            _gunData.Damage += _damageParameter.Up.Value;
            _damageParameter.Info.Value += _damageParameter.Up.Value;

            Updated?.Invoke();
        }
        private void UpgradeCooldown()
        {
            _gunData.Cooldown -= _cooldownParameter.Up.Value;
            _cooldownParameter.Info.Value -= _cooldownParameter.Up.Value;

            Updated?.Invoke();
        }
        private void UpgradeDistance()
        {
            _gunData.Distance += _distanceParameter.Up.Value;
            _distanceParameter.Info.Value += _distanceParameter.Up.Value;

            Updated?.Invoke();
        }
    }
}
