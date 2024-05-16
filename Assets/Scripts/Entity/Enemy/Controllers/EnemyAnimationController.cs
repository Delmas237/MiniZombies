using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyLib
{
    [Serializable]
    public class EnemyAnimationController
    {
        public float AttackSpeedX { get; set; } = 1;

        private HealthController healthController;
        private EnemyMoveController moveController;
        private EnemyAttackController attackController;
        private Animator animator;

        public void Initialize(HealthController _healthController, EnemyMoveController _moveController, EnemyAttackController _attackController, 
            Animator _animator)
        {
            healthController = _healthController;
            moveController = _moveController;
            attackController = _attackController;
            animator = _animator;

            healthController.Died += DeathAnim;
        }

        public void MoveAnim()
        {
            if (healthController.Health > 0)
            {
                if (moveController.Target && moveController.Target.HealthController.Health > 0)
                {
                    if (attackController.IsAttack == false)
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
            if (healthController.Health > 0 && attackController.IsAttack)
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
