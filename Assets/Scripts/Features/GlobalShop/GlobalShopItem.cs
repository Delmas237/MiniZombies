using System;
using TMPro;
using UnityEngine;
using Weapons;

namespace GlobalShopLib
{
    [Serializable]
    public class GlobalShopItem
    {
        [SerializeField] private GunType _type;
        [SerializeField] private GlobalShopParameter _damageParameter;
        [SerializeField] private GlobalShopParameter _cooldownParameter;
        [SerializeField] private GlobalShopParameter _distanceParameter;

        private GunSavableData _gunData;
        private GlobalShopItemData _itemData;

        public event Action<GunSavableData> Updated;

        public GunType Type => _type;

        public void Intialize(GlobalShopItemData data)
        {
            if (GunsDataSaver.GunsSavableData.ContainsKey(_type))
            {
                _gunData = GunsDataSaver.GunsSavableData[_type];
                _itemData = data;

                InitializeValues();

                _damageParameter.PurchaseButton.onClick.AddListener(UpgradeDamage);
                _cooldownParameter.PurchaseButton.onClick.AddListener(UpgradeCooldown);
                _distanceParameter.PurchaseButton.onClick.AddListener(UpgradeDistance);
            }
        }

        private void InitializeValues()
        {
            UpdatePriceText(_damageParameter.PriceText, _itemData.DamageParameter.Price);
            UpdatePriceText(_cooldownParameter.PriceText, _itemData.CooldownParameter.Price);
            UpdatePriceText(_distanceParameter.PriceText, _itemData.DistanceParameter.Price);
            
            UpdateUpText(_damageParameter.UpText, _itemData.DamageParameter.Up);
            UpdateUpText(_cooldownParameter.UpText, _itemData.CooldownParameter.Up);
            UpdateUpText(_distanceParameter.UpText, _itemData.DistanceParameter.Up);

            UpdateInfoText(_damageParameter.InfoText, _gunData.Damage);
            UpdateInfoText(_cooldownParameter.InfoText, _gunData.Cooldown);
            UpdateInfoText(_distanceParameter.InfoText, _gunData.Distance);
        }
        private void UpdatePriceText(TextMeshProUGUI text, float value) => text.text = value + "$";
        private void UpdateUpText(TextMeshProUGUI text, float value) => text.text = value.ToString();
        private void UpdateInfoText(TextMeshProUGUI text, float value) => text.text = Math.Round(value, 2).ToString();

        private void UpgradeDamage() => Upgrade(ref _gunData.Damage, _itemData.DamageParameter.Up, _itemData.DamageParameter.Price, _damageParameter.InfoText);
        private void UpgradeCooldown() => Upgrade(ref _gunData.Cooldown, _itemData.CooldownParameter.Up, _itemData.CooldownParameter.Price, _cooldownParameter.InfoText);
        private void UpgradeDistance() => Upgrade(ref _gunData.Distance, _itemData.DistanceParameter.Up, _itemData.DistanceParameter.Price, _distanceParameter.InfoText);
        private void Upgrade(ref float info, float up, float price, TextMeshProUGUI text)
        {
            if (info > up && Bank.Spend(Mathf.FloorToInt(price)))
            {
                info += up;
                UpdateInfoText(text, info);

                Updated?.Invoke(_gunData);
            }
        }
    }
}
