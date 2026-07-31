using System;
using System.Collections;
using UnityEngine;

namespace EntityLib.Hostile
{
    [Serializable]
    public class EnemyMeleeAttackModule : IEnemyAttackModule, IOnCollisionEnter, IOnCollisionExit, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private int _damage = 15;
        [SerializeField, Range(0.01f, 3f)] private float _baseSpeed = 1f;
        [Space(10)]
        [SerializeField] protected float _baseAttackDelay = 0.9f;
        [SerializeField] protected float _baseCooldown = 0.85f;

        [Space(10), Tooltip("Attack stopping speed divided by attack speed")]
        [SerializeField, Range(0, 3f)] private float _stopAttackSpeedRatio = 0.3f;

        private bool _isAttack;
        private IEntity _targetCollision;

        private Coroutine _attackCoroutine;
        private Coroutine _stopAttackCoroutine;

        private IEntityTargetModule _targetModule;
        private IEnemyMovementModule _moveModule;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (!_enabled)
                        StopAttackImmediately();
                }
            }
        }

        public float Speed { get; set; }

        public bool IsAttack => _isAttack;
        public float BaseSpeed => _baseSpeed;
        public int Damage => _damage;

        [ModuleInject]
        private void Initialize(IEntityTargetModule targetModule, IEnemyMovementModule moveModule)
        {
            _targetModule = targetModule;
            _moveModule = moveModule;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IEntity entity) &&
                entity == _targetModule.Target &&
                entity.HealthModule.Health > 0)
            {
                _targetCollision = entity;
                Attack();
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IEntity entity) &&
                entity == _targetModule.Target)
            {
                float delay = _stopAttackSpeedRatio / Speed;
                StopAttackWithDelay(delay);
            }
        }

        public void Attack()
        {
            if (!Enabled)
                return;

            if (_moveModule.Agent != null && _moveModule.Agent.enabled)
                _moveModule.Agent.isStopped = true;

            _isAttack = true;
            _attackCoroutine = CoroutineHelper.StartRoutine(AttackCoroutine());
        }
        protected IEnumerator AttackCoroutine()
        {
            while (_isAttack)
            {
                yield return new WaitForSeconds(_baseAttackDelay / Speed);

                if (!_isAttack)
                    yield break;

                Perform();

                yield return new WaitForSeconds(_baseCooldown / Speed);
            }
        }

        protected void Perform()
        {
            if (!Enabled)
                return;

            if (_targetCollision == null)
                return;

            if (_targetCollision.HealthModule.Health <= 0)
            {
                StopAttackImmediately();
                return;
            }

            _targetCollision.HealthModule.Decrease(Damage);
        }

        public void StopAttackImmediately()
        {
            _targetCollision = null;

            if (_moveModule.Agent != null && _moveModule.Agent.enabled && _moveModule.Agent.isOnNavMesh)
                _moveModule.Agent.isStopped = false;

            _isAttack = false;

            if (_attackCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            if (_stopAttackCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_stopAttackCoroutine);
                _stopAttackCoroutine = null;
            }
        }
        private void StopAttackWithDelay(float delay)
        {
            if (_stopAttackCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_stopAttackCoroutine);
                _stopAttackCoroutine = null;
            }

            _stopAttackCoroutine = CoroutineHelper.StartRoutine(StopAttackCoroutine(delay));
        }
        private IEnumerator StopAttackCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (!Enabled)
                yield break;

            StopAttackImmediately();
        }

        public void Dispose()
        {
            StopAttackImmediately();
        }
    }
}