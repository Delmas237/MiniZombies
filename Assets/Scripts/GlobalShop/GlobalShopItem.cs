using System;
using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    [Serializable]
    public class GlobalShopItem
    {
        [SerializeField] private GunType gunType;
        private GunData gunData;

        [SerializeField] private GlobalShopParameter damageParameter;
        [SerializeField] private GlobalShopParameter cooldownParameter;
        [SerializeField] private GlobalShopParameter distanceParameter;

        public event Action Updated;

        public void Intialize()
        {
            gunData = GunsManager.Guns[(int)gunType];
            UpdateInfo();

            damageParameter.Initialize();
            cooldownParameter.Initialize();
            distanceParameter.Initialize();

            damageParameter.Purchased += UpgradeDamage;
            cooldownParameter.Purchased += UpgradeCooldown;
            distanceParameter.Purchased += UpgradeDistance;
        }
        public void OnDestroy()
        {
            damageParameter.Purchased -= UpgradeDamage;
            cooldownParameter.Purchased -= UpgradeCooldown;
            distanceParameter.Purchased -= UpgradeDistance;
        }

        private void UpdateInfo()
        {
            damageParameter.Info.Value = gunData.Damage;
            cooldownParameter.Info.Value = gunData.Cooldown;
            distanceParameter.Info.Value = gunData.Distance;

            damageParameter.UpdateText();
            cooldownParameter.UpdateText();
            distanceParameter.UpdateText();
        }

        private void UpgradeDamage()
        {
            gunData.Damage += damageParameter.Up.Value;
            damageParameter.Info.Value += damageParameter.Up.Value;

            Updated?.Invoke();
        }
        private void UpgradeCooldown()
        {
            gunData.Cooldown -= cooldownParameter.Up.Value;
            cooldownParameter.Info.Value -= cooldownParameter.Up.Value;

            Updated?.Invoke();
        }
        private void UpgradeDistance()
        {
            gunData.Distance += distanceParameter.Up.Value;
            distanceParameter.Info.Value += distanceParameter.Up.Value;

            Updated?.Invoke();
        }
    }
}
