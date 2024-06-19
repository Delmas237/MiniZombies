using EventBusLib;
using PlayerLib;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace LocalShopLib
{
    public class LocalShop : MonoBehaviour
    {
        [SerializeField] private List<LocalShopItem> _shopItems;
        [SerializeField] private List<LocalShopWeapon> _shopWeapons;
        [Space(10f)]
        [SerializeField] private PlayerContainer _player;
        [SerializeField] private PlayerGunSlots _playerGunSlots;

        private List<int> _weaponsLvl;

        [Space(10f)]
        [SerializeField] private GameObject _shopButton;
        [SerializeField] private GameObject _shopPanel;

        [SerializeField] private AudioSource _getGunSound;

        private void Start()
        {
            UpdatePrice();

            GunsLvlInitialize();

            EventBus.Subscribe<WaveStartedEvent>(ShopDisable);
            EventBus.Subscribe<WaveFinishedEvent>(ShopEnable);
        }
        private void OnDestroy()
        {
            EventBus.Unsubscribe<WaveStartedEvent>(ShopDisable);
            EventBus.Unsubscribe<WaveFinishedEvent>(ShopEnable);
        }

        private void UpdatePrice()
        {
            for (int i = 0; i < _shopWeapons.Count; i++)
                _shopWeapons[i].PriceText.text = _shopWeapons[i].Price.ToString() + "$";
        }

        private void GunsLvlInitialize()
        {
            _weaponsLvl = new List<int>(_shopWeapons.Count);

            for (int i = 0; i < _shopWeapons.Count; i++)
                _weaponsLvl.Add(-1);
        }

        private void ShopEnable(IEvent e)
        {
            _shopButton.SetActive(true);
        }

        private void ShopDisable(IEvent e)
        {
            _shopButton.SetActive(false);
            _shopPanel.SetActive(false);
        }

        private void Update()
        {
#if UNITY_EDITOR
            Cheats();
#endif
        }

        private void Cheats()
        {
            if (Input.GetKeyDown(KeyCode.N))
                _player.CurrencyController.Add(100);
            if (Input.GetKeyDown(KeyCode.M))
                _player.CurrencyController.Spend(100);
        }

        public void PurchaseGun(int ID)
        {
            if (_weaponsLvl[ID] >= 0)
            {
                GunLvlUp(ID);
                return;
            }

            if (_player.CurrencyController.Coins >= _shopWeapons[ID].Price)
            {
                _player.CurrencyController.Spend(_shopWeapons[ID].Price);

                _weaponsLvl[ID]++;

                UpdateLotText(ID);
                GetGun(ID);
            }
        }
        private void GunLvlUp(int ID)
        {
            if (_player.CurrencyController.Coins >= _shopWeapons[ID].PriceLvlBoost)
            {
                _player.CurrencyController.Spend(_shopWeapons[ID].PriceLvlBoost);

                _weaponsLvl[ID]++;
                _player.WeaponsController.Guns[ID + 1].Damage += _shopWeapons[ID].DamageLvlBoost;

                UpdateLotText(ID);
            }
        }
        public void GetGun(int ID)
        {
            if (_weaponsLvl[ID] >= 0 && _playerGunSlots.GunSlot2 != (GunType)ID + 1)
            {
                _playerGunSlots.SetSecondSlot((GunType)ID + 1);
                _getGunSound.Play();
            }
        }

        private void UpdateLotText(int ID)
        {
            _shopWeapons[ID].PriceText.text = _shopWeapons[ID].PriceLvlBoost + "$";
            _shopWeapons[ID].DamageText.text = $"{_player.WeaponsController.Guns[ID + 1].Damage}dmg";
            _shopWeapons[ID].LvlText.text = $"{_weaponsLvl[ID]} lvl";
        }

        public void PurchaseMedKit(int ID)
        {
            if (_player.CurrencyController.Coins >= _shopItems[ID].Price &&
                _player.HealthController.Health < _player.HealthController.MaxHealth)
            {
                _player.CurrencyController.Spend(_shopItems[ID].Price);
                _player.HealthController.Health = _player.HealthController.MaxHealth;
            }
        }
    }
}
