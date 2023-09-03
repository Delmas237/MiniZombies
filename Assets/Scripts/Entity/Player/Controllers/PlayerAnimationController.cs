using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [RequireComponent(typeof(Player))]
    public class PlayerAnimationController : MonoBehaviour
    {
        private Player player;
        private Animator animator;

        public void Initialize(Player _player)
        {
            player = _player;
            animator = GetComponent<Animator>();

            player.WeaponsController.GunChanged += CurrentGunAnim;
            player.HealthController.Died += DeathAnim;
        }
        private void OnDestroy()
        {
            player.WeaponsController.GunChanged -= CurrentGunAnim;
            player.HealthController.Died -= DeathAnim;
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
