using EventBusLib;
using PlayerLib;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _slotsText;

        private List<int> _weaponsLvl;

        [Space(10f)]
        [SerializeField] private GameObject _shopButton;
        [SerializeField] private GameObject _shopPanel;

        [SerializeField] private AudioSource _getGunSound;

        private void Start()
        {
            UpdatePrice();
            UpdateSlotsText();

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
        private void UpdateSlotsText()
        {
            _slotsText.text = $"Slots used {_playerGunSlots.UsedSlots}/{_playerGunSlots.MaxSlots}";
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

        public void PurchaseGun(int id)
        {
            if (_weaponsLvl[id] >= 0)
            {
                GunLvlUp(id);
                return;
            }

            if (_player.CurrencyController.Spend(_shopWeapons[id].Price))
            {
                _weaponsLvl[id]++;

                GetGun(id);
                UpdateLotText(id);
            }
        }
        private void GunLvlUp(int id)
        {
            if (_player.CurrencyController.Spend(_shopWeapons[id].PriceLvlBoost))
            {
                _weaponsLvl[id]++;
                _player.WeaponsController.Guns[id + 1].Damage += _shopWeapons[id].DamageLvlBoost;

                UpdateLotText(id);
            }
        }
        public void GetGun(int id)
        {
            if (_weaponsLvl[id] >= 0 && _playerGunSlots.SetFreeOrLastSlot((GunType)id + 1))
            {
                _getGunSound.Play();
                UpdateSlotsText();
            }
        }

        private void UpdateLotText(int id)
        {
            _shopWeapons[id].PriceText.text = _shopWeapons[id].PriceLvlBoost + "$";
            _shopWeapons[id].DamageText.text = $"{_player.WeaponsController.Guns[id + 1].Damage}dmg";
            _shopWeapons[id].LvlText.text = $"{_weaponsLvl[id]} lvl";
        }

        public void PurchaseMedKit(int id)
        {
            if (_player.CurrencyController.Spend(_shopItems[id].Price) &&
                _player.HealthController.Health < _player.HealthController.MaxHealth)
            {
                _player.HealthController.Health = _player.HealthController.MaxHealth;
            }
        }
    }
}
