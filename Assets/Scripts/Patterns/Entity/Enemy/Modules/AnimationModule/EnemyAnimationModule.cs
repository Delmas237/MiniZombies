using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyLib
{
    [Serializable]
    public class EnemyAnimationModule
    {
        private IHealthModule _healthModule;
        private IEnemyMoveModule _moveModule;
        private IEnemyAttackModule _attackModule;
        private Animator _animator;

        public const float DEFAULT_MOVE_SPEED = 3.7f;

        public void Initialize(IHealthModule healthModule, IEnemyMoveModule moveModule, IEnemyAttackModule attackModule, 
            Animator animator)
        {
            _healthModule = healthModule;
            _moveModule = moveModule;
            _attackModule = attackModule;
            _animator = animator;

            _healthModule.IsOver += DeathAnim;
        }

        public void UpdateData()
        {
            _animator.SetFloat("MoveSpeed", ConvertMoveSpeed(_moveModule.Speed));
            _animator.SetFloat("AttackSpeed", _attackModule.Speed);
        }

        private float ConvertMoveSpeed(float value)
        {
            float scaleFactor = 0.1f;
            double result = 1 + scaleFactor * (value - DEFAULT_MOVE_SPEED);

            return (float)Math.Round(result, 3);
        }
        public void MoveAnim()
        {
            if (_healthModule.Health > 0)
            {
                if (_moveModule.Target != null && _moveModule.Target.HealthModule.Health > 0)
                {
                    if (_attackModule.IsAttack == false)
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
            if (_healthModule.Health > 0 && _attackModule.IsAttack)
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
            _healthModule.IsOver -= DeathAnim;
        }
    }
}
