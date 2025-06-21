using EventBusLib;
using Factory;
using PlayerLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Weapons;

namespace LocalShopLib
{
    public class LocalShop : MonoBehaviour
    {
        [SerializeField] private List<LocalShopItem> _shopItems;
        [SerializeField] private List<LocalShopGun> _shopWeapons;
        [Space(10f)]
        [SerializeField] private string _dataKey = "LocalShopData";
        [Space(10f)]
        [SerializeField] private PlayerContainer _player;
        [SerializeField] private PlayerGunSlots _playerGunSlots;
        [SerializeField] private TextMeshProUGUI _slotsText;
        [Space(10f)]
        [SerializeField] private GameObject _shopButton;
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private AudioSource _getGunSound;
        [Space(10f)]
        [SerializeField] private TurretFactory _turretFactory;

        private LocalShopData _data;
        private Dictionary<GunType, int> _gunsLvl = new Dictionary<GunType, int>();

        private void Start()
        {
            StartCoroutine(Initialize());
        }
        private IEnumerator Initialize()
        {
            yield return StartCoroutine(LoadData());

            UpdatePrice();
            UpdateSlotsText();

            EventBus.Subscribe<WaveStartedEvent>(ShopDisable);
            EventBus.Subscribe<WaveFinishedEvent>(ShopEnable);
        }
        private IEnumerator LoadData()
        {
            AsyncOperationHandle<LocalShopData> handle = Addressables.LoadAssetAsync<LocalShopData>(_dataKey);

            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
                _data = handle.Result;
            else
                Debug.LogError("Failed to load data");
        }

        private void UpdatePrice()
        {
            foreach (var item in _shopItems)
                item.PriceText.text = _data.Items.First(i => i.Name == item.Name).Price.ToString() + "$";

            foreach (var gun in _shopWeapons)
                gun.PriceText.text = _data.Weapons.First(g => g.Type == gun.Type).Price.ToString() + "$";
        }
        private void UpdateSlotsText()
        {
            _slotsText.text = $"Slots used {_playerGunSlots.UsedSlots}/{_playerGunSlots.MaxSlots}";
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
                _player.CurrencyModule.Add(100);
            if (Input.GetKeyDown(KeyCode.M))
                _player.CurrencyModule.Spend(100);
        }

        public void PurchaseGun(int id) => PurchaseGun((GunType)id);
        private void PurchaseGun(GunType type)
        {
            if (_gunsLvl.ContainsKey(type) && _gunsLvl[type] >= 0)
            {
                GunLvlUp(type);
                return;
            }

            LocalShopGunData weaponData = _data.Weapons.First(g => g.Type == type);

            if (_player.CurrencyModule.Spend(weaponData.Price))
            {
                if (!_gunsLvl.ContainsKey(type))
                    _gunsLvl.Add(type, 0);

                _gunsLvl[type]++;

                GetGun(type);
                UpdateLotText(type);
            }
        }
        private void GunLvlUp(GunType type)
        {
            LocalShopGunData weaponData = _data.Weapons.First(g => g.Type == type);

            if (_player.CurrencyModule.Spend(weaponData.PriceLvlBoost))
            {
                _gunsLvl[type]++;
                _player.WeaponsModule.Guns.First(g => g.Type == type).Damage += weaponData.DamageLvlBoost;

                UpdateLotText(type);
            }
        }

        public void GetGun(int id) => GetGun((GunType)id);
        private void GetGun(GunType type)
        {
            if (_gunsLvl.ContainsKey(type) && _gunsLvl[type] >= 0 && _playerGunSlots.SetFreeOrLastSlot(type))
            {
                _getGunSound.Play();
                UpdateSlotsText();
            }
        }

        private void UpdateLotText(GunType type)
        {
            LocalShopGun weapon = _shopWeapons.First(g => g.Type == type);
            LocalShopGunData weaponData = _data.Weapons.First(g => g.Type == type);

            weapon.PriceText.text = weaponData.PriceLvlBoost + "$";
            weapon.DamageText.text = $"{_player.WeaponsModule.Guns.First(g => g.Type == type).Damage}dmg";
            weapon.LvlText.text = $"{_gunsLvl[type]} lvl";
        }

        public void PurchaseItem(string id)
        {
            LocalShopItemData itemData = _data.Items.First(g => g.Name == id);
            
            if (_player.CurrencyModule.IsCanSpend(itemData.Price))
            {
                if (itemData.Name == "Medkit" && _player.HealthModule.Health < _player.HealthModule.MaxHealth)
                {
                    _player.HealthModule.Increase(_player.HealthModule.MaxHealth);
                    _player.CurrencyModule.Spend(itemData.Price);
                }
                else if (itemData.Name == "Turret")
                {
                    TurretContainer turretContainer = _turretFactory.GetInstance();
                    turretContainer.transform.position = _player.Transform.position;
                    _player.CurrencyModule.Spend(itemData.Price);
                }
            }
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<WaveStartedEvent>(ShopDisable);
            EventBus.Unsubscribe<WaveFinishedEvent>(ShopEnable);
        }
    }
}
