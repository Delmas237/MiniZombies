using JoystickLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [Serializable]
    public class PlayerWeaponsController : IPlayerWeaponsController
    {
        [SerializeField] private int _bullets = 100;
        public int Bullets => _bullets;
        public event Action<int> BulletsChanged;

        [SerializeField] private List<Gun> _guns;
        public IReadOnlyList<Gun> Guns => _guns;

        public Gun CurrentGun { get; private set; }
        public event Action<Gun> GunChanged;

        [field: SerializeField] public Joystick AttackJoystick { get; private set; }
        [Space(10)]
        [SerializeField] private Transform _shootLineRoot;

        private IHealthController _healthController;
        private IPlayerMoveController _moveController;

        public void Initialize(IHealthController healthController, IPlayerMoveController moveController)
        {
            _healthController = healthController;
            _moveController = moveController;

            ChangeGun(GunType.Pistol);

            _healthController.Died += OnDeath;
            AttackJoystick.OnUp += PullTrigger;
            AttackJoystick.OnClamped += PullAutoTrigger;
        }
        private void OnDeath()
        {
            _healthController.Died -= OnDeath;
            AttackJoystick.OnUp -= PullTrigger;
            AttackJoystick.OnClamped -= PullAutoTrigger;
        }

        private void PullTrigger()
        {
            if (Bullets - CurrentGun.Consumption >= 0)
            {
                if (CurrentGun.ShootRequest())
                    SpendBullets(CurrentGun.Consumption);
            }
        }
        private void PullAutoTrigger()
        {
            if (Bullets - CurrentGun.Consumption >= 0
                && CurrentGun.FireType == GunFireType.Auto)
            {
                if (CurrentGun.ShootRequest())
                    SpendBullets(CurrentGun.Consumption);
            }
        }

        public void ChangeGun(GunType gunType)
        {
            CurrentGun = _guns[(int)gunType];
            GunChanged.Invoke(CurrentGun);

            UpdateGunsVisible();
            UpdateShootLineScale();

            _moveController.AutoRotate = CurrentGun.FireType == GunFireType.Auto;
        }

        private void UpdateGunsVisible()
        {
            int currentGunIndex = (int)CurrentGun.Type;
            
            for (int i = 0; i < Guns.Count; i++)
                _guns[i].gameObject.SetActive(i == currentGunIndex);
        }

        private void UpdateShootLineScale()
        {
            _shootLineRoot.localScale = new Vector3(1, 1, CurrentGun.Distance);
        }

        public void UpdateShootLine()
        {
            if (AttackJoystick.Direction != Vector2.zero)
            {
                _shootLineRoot.gameObject.SetActive(true);
                _shootLineRoot.rotation = Quaternion.LookRotation(new Vector3(AttackJoystick.Horizontal, 0, AttackJoystick.Vertical));
            }
            else
            {
                _shootLineRoot.gameObject.SetActive(false);
            }
        }

        public void AddBullets(int amount)
        {
            if (amount <= 0)
                return;

            _bullets += amount;
            BulletsChanged?.Invoke(_bullets);
        }

        public void SpendBullets(int amount)
        {
            if (amount <= 0)
                return;

            _bullets -= amount;
            BulletsChanged?.Invoke(_bullets);
        }
    }
}
