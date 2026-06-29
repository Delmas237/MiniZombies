using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity.Hostile
{
    [Serializable]
    public class EnemyAnimationModule : IEntityModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;

        private IEntityHealthModule _healthModule;
        private IEntityTargetModule _targetModule;
        private IEnemyMovementModule _moveModule;
        private IEnemyAttackModule _attackModule;
        private Animator _animator;

        public const float DEFAULT_MOVE_SPEED = 3.7f;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        public void Initialize(Animator animator, IEntityHealthModule healthModule, IEntityTargetModule targetModule, IEnemyMovementModule moveModule,  IEnemyAttackModule attackModule)
        {
            _animator = animator;

            _healthModule = healthModule;
            _targetModule = targetModule;
            _moveModule = moveModule;
            _attackModule = attackModule;

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
            if (!_enabled)
                return;

            if (_healthModule.Health <= 0)
                return;

            if (_targetModule.Target != null && _targetModule.Target.HealthModule.Health > 0)
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

        public void AttackAnim()
        {
            if (!_enabled)
                return;

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
            if (!_enabled)
                return;

            int rnd = Random.Range(1, 3);
            _animator.SetBool("Death" + rnd, true);
        }

        public void Dispose()
        {
            _healthModule.IsOver -= DeathAnim;
        }
    }
}
