using System;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [Serializable]
    public class PlayerAnimationController
    {
        private PlayerContainer player;
        private Animator animator;

        public void Initialize(PlayerContainer _player, Animator _animator)
        {
            player = _player;
            animator = _animator;

            player.WeaponsController.GunChanged += CurrentGunAnim;
            player.HealthController.Died += DeathAnim;
        }

        public void MoveAnim()
        {
            if (player.MoveController.Walks)
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

        private void CurrentGunAnim()
        {
            if (player.WeaponsController.CurrentGun.Type == GunType.Pistol)
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
