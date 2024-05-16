using System;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [Serializable]
    public class PlayerAnimationController
    {
        private Animator animator;

        private HealthController healthController;
        private PlayerWeaponsController weaponsController;
        private PlayerMoveController moveController;

        public void Initialize(HealthController _healthController, PlayerWeaponsController _weaponsController, PlayerMoveController _moveController, 
            Animator _animator)
        {
            healthController = _healthController;
            weaponsController = _weaponsController;
            moveController = _moveController;

            animator = _animator;

            weaponsController.GunChanged += CurrentGunAnim;
            healthController.Died += DeathAnim;
            healthController.Died += OnDeath;
        }
        private void OnDeath()
        {
            weaponsController.GunChanged -= CurrentGunAnim;
            healthController.Died -= DeathAnim;
            healthController.Died -= OnDeath;
        }

        public void MoveAnim()
        {
            if (moveController.Walks)
            {
                animator.SetFloat("SpeedPistol", 1);
                animator.SetFloat("Speed", 1.7f);
            }
            else
            {
                animator.SetFloat("SpeedPistol", 0);
                animator.SetFloat("Speed", 0);
            }
        }

        private void CurrentGunAnim(Gun gun)
        {
            if (gun.Type == GunType.Pistol)
            {
                animator.SetBool("PistolInHands", true);
            }
            else
            {
                animator.SetBool("PistolInHands", false);
            }
        }

        private void DeathAnim() => animator.SetBool("Died", true);
    }
}
