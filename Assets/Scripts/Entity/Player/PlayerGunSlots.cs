using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    public class PlayerGunSlots : MonoBehaviour
    {
        [SerializeField] private PlayerContainer _player;

        [SerializeField] private List<GameObject> _gunsSlot2;

        public GunType GunSlot1 { get; private set; } = GunType.Pistol;
        public GunType GunSlot2 { get; private set; }

        [SerializeField] private AudioSource _getGunSound;

        private void Start()
        {
            UpdateSlotsImages();
        }

        public void SetSecondSlot(GunType gunType)
        {
            GunSlot2 = gunType;
            _player.WeaponsController.ChangeGun(gunType);

            UpdateSlotsImages();
        }

        public void ChangeGunSlot(int slot)
        {
            if (slot == 1 && _player.WeaponsController.CurrentGun.Type != GunSlot1)
            {
                _player.WeaponsController.ChangeGun(GunSlot1);
                _getGunSound.Play();
            }
            if (slot == 2 && _player.WeaponsController.CurrentGun.Type != GunSlot2)
            {
                _player.WeaponsController.ChangeGun(GunSlot2);
                _getGunSound.Play();
            }
            UpdateSlotsImages();
        }

        private void UpdateSlotsImages()
        {
            int current = (int)GunSlot2 - 1;
            for (int i = 0; i < _gunsSlot2.Count; i++)
            {
                _gunsSlot2[i].SetActive(i == current);
            }
        }
    }
}
