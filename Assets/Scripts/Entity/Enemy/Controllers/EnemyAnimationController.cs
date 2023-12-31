using UnityEngine;

namespace EnemyLib
{
    public class EnemyAnimationController : MonoBehaviour
    {
        public float AttackSpeedX { get; set; } = 1;

        private Enemy enemy;
        private Animator animator;

        public void Initialize(Enemy _enemy)
        {
            enemy = _enemy;
            animator = enemy.GetComponent<Animator>();

            enemy.HealthController.Died += DeathAnim;
        }

        public void MoveAnim()
        {
            if (enemy.HealthController.Health > 0)
            {
                if (enemy.MoveController.Target && enemy.MoveController.Target.HealthController.Health > 0)
                {
                    if (enemy.AttackController.IsAttack == false)
                        animator.SetBool("Run", true);

                    animator.SetBool("Idle", false);
                }
                else
                {
                    animator.SetBool("Idle", true);
                    animator.SetBool("Run", false);
                }
            }
        }

        public void AttackAnim()
        {
            if (enemy.HealthController.Health > 0 && enemy.AttackController.IsAttack)
            {
                animator.SetFloat("AttackSpeed", AttackSpeedX);
                animator.SetBool("Attack", true);
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }

        private void DeathAnim()
        {
            int rnd = Random.Range(1, 3);
            animator.SetBool("Death" + rnd, true);
        }
    }
}
