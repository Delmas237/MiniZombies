using Saves;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace EntityLib.Friendly.Player
{
    public class PlayerGunSlots : MonoBehaviour
    {
        [SerializeField] private List<Image> _slotsImages;
        [SerializeField] private AudioSource _getGunSound;
        [Space(10)]
        [SerializeField] private Entity _player;

        private bool _isInitialized;
        private List<GunType?> _slots = new List<GunType?>();

        public Action Initialized;

        public bool IsInitialized => _isInitialized;
        public IReadOnlyList<GunType?> Slots => _slots;
        public int UsedSlots => _slots.Count;
        public int MaxSlots => _slotsImages.Count;

        private void Start()
        {
            if (GunsDataSaver.IsInitialized)
                Initialize();
            else
            {
                GunsDataSaver.Initialize();
                GunsDataSaver.Initialized += Initialize;
            }
        }

        private void Initialize()
        {
            GunsDataSaver.Initialized -= Initialize;

            SetInitialGun();
            UpdateSlotsImages();

            _isInitialized = true;
            Initialized?.Invoke();
        }

        private void SetInitialGun()
        {
            var weaponModule = _player.GetModule<IEntityWeaponModule>();

            GunType initialGun = weaponModule.InitialGun;
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

            var weaponModule = _player.GetModule<IEntityWeaponModule>();
            weaponModule.ChangeGun(gunType);

            return true;
        }

        public void ChangeCurrentGun(int slot)
        {
            var weaponModule = _player.GetModule<IEntityWeaponModule>();
            if (weaponModule.CurrentGun.Type == _slots[slot])
                return;

            weaponModule.ChangeGun((GunType)_slots[slot]);
            _getGunSound.Play();
        }
    }
}
