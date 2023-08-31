using System;
using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    public class GlobalShopItem : MonoBehaviour
    {
        [SerializeField] private GunType gunType;

        [SerializeField] private GlobalShopParameter damageParameter;
        [SerializeField] private GlobalShopParameter cooldownParameter;
        [SerializeField] private GlobalShopParameter distanceParameter;

        public event Action Updated;

        private void Start()
        {
            UpdateInfo();

            damageParameter.Purchased += UpDamage;
            cooldownParameter.Purchased += UpCooldown;
            distanceParameter.Purchased += UpDistance;
        }
        private void OnDestroy()
        {
            damageParameter.Purchased -= UpDamage;
            cooldownParameter.Purchased -= UpCooldown;
            distanceParameter.Purchased -= UpDistance;
        }

        private void UpdateInfo()
        {
            damageParameter.Info = GunsManager.Guns[(int)gunType].Damage;
            cooldownParameter.Info = GunsManager.Guns[(int)gunType].Cooldown;
            distanceParameter.Info = GunsManager.Guns[(int)gunType].Distance;

            damageParameter.UpdateText();
            cooldownParameter.UpdateText();
            distanceParameter.UpdateText();
        }

        private void UpDamage()
        {
            GunsManager.Guns[(int)gunType].Damage += damageParameter.Up;
            damageParameter.Info += damageParameter.Up;

            Updated?.Invoke();
        }
        private void UpCooldown()
        {
            GunsManager.Guns[(int)gunType].Cooldown -= cooldownParameter.Up;
            cooldownParameter.Info -= cooldownParameter.Up;

            Updated?.Invoke();
        }
        private void UpDistance()
        {
            GunsManager.Guns[(int)gunType].Distance += distanceParameter.Up;
            distanceParameter.Info += distanceParameter.Up;

            Updated?.Invoke();
        }
    }
}
