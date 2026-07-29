using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity.Hostile
{
    [Serializable]
    public class EnemyAnimationModule : IModule, IOnEnable, ILateUpdatable, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [SerializeField, Range(1, 2)] private int _deathAnimationsCount = 2;

        private Animator _animator;
        private IEntityHealthModule _healthModule;
        private IEntityTargetModule _targetModule;
        private IEnemyMovementModule _moveModule;
        private IEnemyAttackModule _attackModule;

        public const float DEFAULT_MOVE_SPEED = 3.7f;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        [ModuleInject]
        private void Initialize(Animator animator, IEntityHealthModule healthModule, IEntityTargetModule targetModule, IEnemyMovementModule moveModule, IEnemyAttackModule attackModule)
        {
            _animator = animator;

            _healthModule = healthModule;
            _targetModule = targetModule;
            _moveModule = moveModule;
            _attackModule = attackModule;

            _healthModule.IsOver += DeathAnim;
        }

        public void OnEnable()
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

        public void LateUpdate()
        {
            MoveAnim();
            AttackAnim();
        }

        private void MoveAnim()
        {
            if (_healthModule.Health <= 0)
                return;

            if (_targetModule.Target != null && _targetModule.Target.HealthModule.Health > 0)
            {
                if (!_attackModule.IsAttack)
                    _animator.SetBool("Run", true);

                _animator.SetBool("Idle", false);
            }
            else
            {
                _animator.SetBool("Idle", true);
                _animator.SetBool("Run", false);
            }
        }

        private void AttackAnim()
        {
            _animator.SetBool("Attack", _healthModule.Health > 0 && _attackModule.IsAttack);
        }

        private void DeathAnim()
        {
            if (!Enabled)
                return;

            int rnd = Random.Range(1, _deathAnimationsCount + 1);
            _animator.SetBool("Death" + rnd, true);
        }

        public void Dispose()
        {
            if (_healthModule != null)
                _healthModule.IsOver -= DeathAnim;
        }
    }
}
