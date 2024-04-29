using JoystickLib;
using System;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [Serializable]
    public class PlayerWeaponsController
    {
        [field: SerializeField] public int Bullets { get; set; } = 100;

        [field: SerializeField] public Gun[] Guns { get; private set; }
        public Gun CurrentGun { get; private set; }
        public event Action GunChanged;

        [field: SerializeField] public Joystick AttackJoystick { get; private set; }

        private PlayerContainer player;

        public void Initialize(PlayerContainer _player)
        {
            player = _player;

            ChangeGun(GunType.Pistol);

            player.HealthController.Died += OnDeath;
            AttackJoystick.OnUpNotInDeadZone += player.MoveController.RotateToAttackJoystickDir;
            AttackJoystick.OnUp += PullTrigger;
            AttackJoystick.OnClamped += PullAutoTrigger;
        }
        public void OnDeath()
        {
            player.HealthController.Died -= OnDeath;
            AttackJoystick.OnUpNotInDeadZone -= player.MoveController.RotateToAttackJoystickDir;
            AttackJoystick.OnUp -= PullTrigger;
            AttackJoystick.OnClamped -= PullAutoTrigger;
        }

        private void PullTrigger()
        {
            if (Bullets - CurrentGun.Consumption >= 0)
            {
                if (CurrentGun.ShootRequest())
                    Bullets -= CurrentGun.Consumption;
            }
        }
        private void PullAutoTrigger()
        {
            if (Bullets - CurrentGun.Consumption >= 0
                && CurrentGun.Type == GunType.MachineGun)
            {
                if (CurrentGun.ShootRequest())
                    Bullets -= CurrentGun.Consumption;
            }
        }

        public void ChangeGun(GunType gunType)
        {
            CurrentGun = Guns[(int)gunType];
            GunChanged.Invoke();

            UpdateGuns(gunType);

            player.MoveController.AutoRotate = gunType switch
            {
                GunType.MachineGun => true,
                _ => false,
            };
        }

        private void UpdateGuns(GunType gunInHands)
        {
            for (int i = 0; i < Guns.Length; i++)
            {
                if (i == (int)gunInHands)
                    Guns[i].gameObject.SetActive(true);
                else
                    Guns[i].gameObject.SetActive(false);
            }
        }
    }
}
