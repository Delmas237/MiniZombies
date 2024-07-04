using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace PlayerLib
{
    public class PlayerGunSlots : MonoBehaviour
    {
        private List<GunType?> _slots = new List<GunType?>();
        public IReadOnlyList<GunType?> Slots => _slots;

        public int UsedSlots => _slots.Count;
        public int MaxSlots => _slotsImages.Count;

        [SerializeField] private List<Image> _slotsImages;
        [SerializeField] private AudioSource _getGunSound;
        [Space(10)]
        [SerializeField] private PlayerContainer _player;

        private void Start()
        {
            SetInitialGun();
            UpdateSlotsImages();
        }

        private void SetInitialGun()
        {
            GunType initialGun = _player.WeaponsController.InitialGun;
            _slots.Add(initialGun);
            _slotsImages[_slots.Count - 1].sprite = GunsDataSaver.GunsData[initialGun].Icon;
        }

        private void UpdateSlotsImages()
        {
            for (int i = 0; i < _slotsImages.Count; i++)
            {
                if (i >= _slots.Count)
                    _slotsImages[i].enabled = false;
            }
        }

        public bool SetFreeOrLastSlot(GunType gunType)
        {
            if (_slots.Contains(gunType))
                return false;

            int index;
            Sprite sprite = GunsDataSaver.GunsData[gunType].Icon;
            if (_slots.Count != _slotsImages.Count)
            {
                _slots.Add(gunType);
                index = _slots.IndexOf(gunType);
            }
            else
            {
                index = _slots.Count - 1;
                _slots[index] = gunType;
            }
            _slotsImages[index].sprite = sprite;
            _slotsImages[index].enabled = true;

            _player.WeaponsController.ChangeGun(gunType);

            return true;
        }

        public void ChangeCurrentGun(int slot)
        {
            if (_player.WeaponsController.CurrentGun.Type == _slots[slot])
                return;

            _player.WeaponsController.ChangeGun((GunType)_slots[slot]);
            _getGunSound.Play();
        }
    }
}
