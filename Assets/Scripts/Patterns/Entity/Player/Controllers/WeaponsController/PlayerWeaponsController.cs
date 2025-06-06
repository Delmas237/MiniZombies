using JoystickLib;
using System;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [Serializable]
    public class PlayerWeaponsController : WeaponsController, IPlayerWeaponsController
    {
        [SerializeField] private Transform _shootLineRoot;
        [SerializeField] private Joystick _attackJoystick;
        private IHealthController _healthController;

        public const float START_DISTANCE = 0.848f;

        public Joystick AttackJoystick => _attackJoystick;

        public void Initialize(IHealthController healthController)
        {
            _healthController = healthController;

            base.Initialize();

            _healthController.IsOver += OnHealhIsOver;
            AttackJoystick.OnUp += PullTrigger;
            AttackJoystick.OnClamped += PullAutoTrigger;
        }
        protected void OnHealhIsOver()
        {
            _healthController.IsOver -= OnHealhIsOver;
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
