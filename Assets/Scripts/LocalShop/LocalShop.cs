using EnemyLib;
using PlayerLib;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace LocalShopLib
{
    public class LocalShop : MonoBehaviour
    {
        [SerializeField] private LocalShopItem[] shopItems;
        [SerializeField] private LocalShopWeapon[] shopWeapons;
        [Space(10f)]
        [SerializeField] private Player player;
        [SerializeField] private PlayerGunSlots playerGunSlots;

        private List<int> weaponsLvl;

        [Space(10f)]
        [SerializeField] private GameObject shopButton;
        [SerializeField] private GameObject shopPanel;

        [SerializeField] private AudioSource getGunSound;

        private void Start()
        {
            UpdatePrice();

            GunsLvlInitialize();

            EnemyWaveManager.WaveStarted += ShopDisable;
            EnemyWaveManager.WaveFinished += ShopEnable;
        }

        private void UpdatePrice()
        {
            for (int i = 0; i < shopWeapons.Length; i++)
                shopWeapons[i].PriceText.text = shopWeapons[i].Price.ToString() + "$";
        }

        private void GunsLvlInitialize()
        {
            weaponsLvl = new List<int>(shopWeapons.Length);

            for (int i = 0; i < shopWeapons.Length; i++)
                weaponsLvl.Add(-1);
        }

        private void OnDestroy()
        {
            EnemyWaveManager.WaveStarted -= ShopDisable;
            EnemyWaveManager.WaveFinished -= ShopEnable;
        }

        private void ShopEnable()
        {
            shopButton.SetActive(true);
        }
        private void ShopDisable()
        {
            shopButton.SetActive(false);
            shopPanel.SetActive(false);
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
                player.Coins += 100;
            if (Input.GetKeyDown(KeyCode.M))
                player.Coins -= 100;
        }

        public void PurchaseGun(int ID)
        {
            if (weaponsLvl[ID] >= 0)
            {
                GunLvlUp(ID);
                return;
            }

            if (player.Coins >= shopWeapons[ID].Price)
            {
                player.Coins -= shopWeapons[ID].Price;

                weaponsLvl[ID]++;

                UpdateLotText(ID);
                GetGun(ID);
            }
        }
        private void GunLvlUp(int ID)
        {
            if (player.Coins >= 100)
            {
                player.Coins -= 100;

                weaponsLvl[ID]++;
                player.WeaponsController.Guns[ID + 1].Damage += shopWeapons[ID].DamageLvlBoost;

                UpdateLotText(ID);
            }
        }
        public void GetGun(int ID)
        {
            if (weaponsLvl[ID] >= 0 && playerGunSlots.GunSlot2 != (GunType)ID + 1)
            {
                playerGunSlots.GunSlot2 = (GunType)ID + 1;
                player.WeaponsController.ChangeGun(playerGunSlots.GunSlot2);

                getGunSound.Play();
            }
        }

        private void UpdateLotText(int ID)
        {
            shopWeapons[ID].PriceText.text = "100$";
            shopWeapons[ID].DamageText.text = $"{player.WeaponsController.Guns[ID + 1].Damage}dmg";
            shopWeapons[ID].LvlText.text = $"{weaponsLvl[ID]} lvl";
        }

        public void PurchaseMedKit(int ID)
        {
            if (player.Coins >= shopItems[ID].Price &&
                player.HealthController.Health < player.HealthController.MaxHealth)
            {
                player.Coins -= shopItems[ID].Price;
                player.HealthController.Health = player.HealthController.MaxHealth;
            }
        }
    }
}
