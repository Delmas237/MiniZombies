using JoystickLib;
using System;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [Serializable]
    public class PlayerWeaponsController : WeaponsController, IPlayerWeaponsController
    {
        [field: SerializeField] public Joystick AttackJoystick { get; private set; }
        [Space(10)]
        [SerializeField] private Transform _shootLineRoot;

        public override void Initialize(IHealthController healthController)
        {
            base.Initialize(healthController);

            AttackJoystick.OnUp += PullTrigger;
            AttackJoystick.OnClamped += PullAutoTrigger;
        }
        protected override void OnDeath()
        {
            base.OnDeath();

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
            _shootLineRoot.localScale = new Vector3(1, 1, CurrentGun.Distance);
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
