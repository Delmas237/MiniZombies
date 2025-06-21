using JoystickLib;
using System;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [Serializable]
    public class PlayerWeaponsModule : WeaponsModule, IPlayerWeaponsModule
    {
        [SerializeField] private Transform _shootLineRoot;
        [SerializeField] private Joystick _attackJoystick;
        private IHealthModule _healthModule;

        public const float START_DISTANCE = 0.848f;

        public Joystick AttackJoystick => _attackJoystick;

        public void Initialize(IHealthModule healthModule)
        {
            _healthModule = healthModule;

            base.Initialize();

            _healthModule.IsOver += OnHealhIsOver;
            AttackJoystick.OnUp += PullTrigger;
            AttackJoystick.OnClamped += PullAutoTrigger;
        }
        private void PullAutoTrigger()
        {
            if (CurrentGun.FireType == GunFireType.Auto)
                PullTrigger();
        }
        protected void OnHealhIsOver()
        {
            _healthModule.IsOver -= OnHealhIsOver;
            AttackJoystick.OnUp -= PullTrigger;
            AttackJoystick.OnClamped -= PullAutoTrigger;
        }

        public override void ChangeGun(GunType gunType)
        {
            base.ChangeGun(gunType);

            UpdateShootLineScale();
        }

        private void UpdateShootLineScale()
        {
            float distance = CurrentGun.Distance + START_DISTANCE;
            _shootLineRoot.localScale = new Vector3(1, 1, distance);
        }

        public void UpdateShootLine()
        {
            bool attackJoystickMoving = AttackJoystick.Direction != Vector2.zero;
            if (attackJoystickMoving)
            {
                _shootLineRoot.rotation = Quaternion.LookRotation(new Vector3(AttackJoystick.Horizontal, 0, AttackJoystick.Vertical));
            }
            _shootLineRoot.gameObject.SetActive(attackJoystickMoving);
        }
    }
}
