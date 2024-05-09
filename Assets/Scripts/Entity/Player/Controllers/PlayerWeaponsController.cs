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
        public event Action<Gun> GunChanged;

        [field: SerializeField] public Joystick AttackJoystick { get; private set; }
        [Space(10)]
        [SerializeField] private Transform shootLineRoot;

        private PlayerContainer player;

        public void Initialize(PlayerContainer _player)
        {
            player = _player;

            ChangeGun(GunType.Pistol);

            player.HealthController.Died += OnDeath;
            AttackJoystick.OnUp += PullTrigger;
            AttackJoystick.OnClamped += PullAutoTrigger;
        }
        private void OnDeath()
        {
            player.HealthController.Died -= OnDeath;
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
                && CurrentGun.FireType == GunFireType.Auto)
            {
                if (CurrentGun.ShootRequest())
                    Bullets -= CurrentGun.Consumption;
            }
        }

        public void ChangeGun(GunType gunType)
        {
            CurrentGun = Guns[(int)gunType];
            GunChanged.Invoke(CurrentGun);

            UpdateGunsVisible();
            UpdateShootLineScale();

            player.MoveController.AutoRotate = CurrentGun.FireType == GunFireType.Auto;
        }

        private void UpdateGunsVisible()
        {
            int currentGunIndex = (int)CurrentGun.Type;
            
            for (int i = 0; i < Guns.Length; i++)
                Guns[i].gameObject.SetActive(i == currentGunIndex);
        }

        private void UpdateShootLineScale()
        {
            shootLineRoot.localScale = new Vector3(1, 1, CurrentGun.Distance);
        }

        public void UpdateShootLine()
        {
            if (AttackJoystick.Direction != Vector2.zero)
            {
                shootLineRoot.gameObject.SetActive(true);
                shootLineRoot.rotation = Quaternion.LookRotation(new Vector3(AttackJoystick.Horizontal, 0, AttackJoystick.Vertical));
            }
            else
            {
                shootLineRoot.gameObject.SetActive(false);
            }
        }
    }
}
