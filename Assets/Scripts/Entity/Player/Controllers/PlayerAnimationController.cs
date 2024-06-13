using System;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [Serializable]
    public class PlayerAnimationController
    {
        private Animator _animator;

        private IHealthController _healthController;
        private IWeaponsController _weaponsController;
        private IPlayerMoveController _moveController;

        public void Initialize(IHealthController healthController, IWeaponsController weaponsController, IPlayerMoveController moveController, 
            Animator animator)
        {
            _healthController = healthController;
            _weaponsController = weaponsController;
            _moveController = moveController;

            _animator = animator;

            _weaponsController.GunChanged += CurrentGunAnim;
            _healthController.Died += DeathAnim;
            _healthController.Died += OnDeath;
        }
        private void OnDeath()
        {
            _weaponsController.GunChanged -= CurrentGunAnim;
            _healthController.Died -= DeathAnim;
            _healthController.Died -= OnDeath;
        }

        public void MoveAnim()
        {
            if (_moveController.IsMoving)
            {
                _animator.SetFloat("SpeedPistol", 1);
                _animator.SetFloat("Speed", 1.7f);
            }
            else
            {
                _animator.SetFloat("SpeedPistol", 0);
                _animator.SetFloat("Speed", 0);
            }
        }

        private void CurrentGunAnim(Gun gun)
        {
            _animator.SetBool("PistolInHands", gun.Type == GunType.Pistol);
        }

        private void DeathAnim() => _animator.SetBool("Died", true);
    }
}
