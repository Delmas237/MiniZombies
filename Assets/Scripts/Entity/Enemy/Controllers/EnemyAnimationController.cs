using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyLib
{
    [Serializable]
    public class EnemyAnimationController
    {
        private IHealthController _healthController;
        private IEnemyMoveController _moveController;
        private IEnemyAttackController _attackController;
        private Animator _animator;

        public const float DEFAULT_MOVE_SPEED = 3.7f;

        public void Initialize(IHealthController healthController, IEnemyMoveController moveController, IEnemyAttackController attackController, 
            Animator animator)
        {
            _healthController = healthController;
            _moveController = moveController;
            _attackController = attackController;
            _animator = animator;

            _healthController.Died += DeathAnim;
        }

        public void OnEnable()
        {
            _animator.SetFloat("MoveSpeed", ConvertMoveSpeed(_moveController.Speed));
            _animator.SetFloat("AttackSpeed", _attackController.AttackSpeed);
        }
        private float ConvertMoveSpeed(float value)
        {
            float scaleFactor = 0.1f; // Коэффициент масштабирования
            double result = 1 + scaleFactor * (value - DEFAULT_MOVE_SPEED);

            return (float)Math.Round(result, 3);
        }
        public void MoveAnim()
        {
            if (_healthController.Health > 0)
            {
                if (_moveController.Target != null && _moveController.Target.HealthController.Health > 0)
                {
                    if (_attackController.IsAttack == false)
                        _animator.SetBool("Run", true);

                    _animator.SetBool("Idle", false);
                }
                else
                {
                    _animator.SetBool("Idle", true);
                    _animator.SetBool("Run", false);
                }
            }
        }

        public void AttackAnim()
        {
            if (_healthController.Health > 0 && _attackController.IsAttack)
            {
                _animator.SetBool("Attack", true);
            }
            else
            {
                _animator.SetBool("Attack", false);
            }
        }

        private void DeathAnim()
        {
            int rnd = Random.Range(1, 3);
            _animator.SetBool("Death" + rnd, true);
        }

        public void OnDestroy()
        {
            _healthController.Died -= DeathAnim;
        }
    }
}
