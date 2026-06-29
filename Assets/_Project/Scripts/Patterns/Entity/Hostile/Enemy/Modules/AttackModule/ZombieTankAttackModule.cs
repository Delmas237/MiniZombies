using System;
using System.Collections;
using UnityEngine;

namespace Entity.Hostile
{
    [Serializable]
    public class ZombieTankAttackModule : IEnemyAttackModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField, Range(0.01f, 3f)] private float _defaultSpeed = 1f;
        [SerializeField] private int _damage = 15;

        [Space(10), Tooltip("Attack stopping speed divided by attack speed")]
        [SerializeField, Range(0, 3f)] private float _stopAttackSpeedRatio = 0.3f;

        private bool _isAttack;
        private IEntity _targetCollision;
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
        public float DefaultSpeed => _defaultSpeed;
        public int Damage => _damage;

        public void Initialize(IEntityTargetModule targetModule, IEnemyMovementModule moveModule)
        {
            _targetModule = targetModule;
            _moveModule = moveModule;

            Speed = DefaultSpeed;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (!_enabled)
                return;

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
            if (!_enabled)
                return;

            if (collision.gameObject.TryGetComponent(out IEntity entity) &&
                entity == _targetModule.Target)
            {
                float delay = _stopAttackSpeedRatio / Speed;
                StopAttackWithDelay(delay);
            }
        }

        private void Attack()
        {
            if (!_enabled)
                return;

            if (_moveModule.Agent != null && _moveModule.Agent.enabled)
                _moveModule.Agent.isStopped = true;

            _isAttack = true;
        }

        private void StopAttackWithDelay(float delay)
        {
            if (!_enabled)
                return;

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

            if (!_enabled)
                yield break;

            StopAttackImmediately();
        }

        public void StopAttackImmediately()
        {
            _targetCollision = null;

            if (_moveModule.Agent != null && _moveModule.Agent.enabled && _moveModule.Agent.isOnNavMesh)
                _moveModule.Agent.isStopped = false;

            _isAttack = false;

            if (_stopAttackCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_stopAttackCoroutine);
                _stopAttackCoroutine = null;
            }
        }

        public void DealDamage()
        {
            if (!_enabled)
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

        public void Dispose()
        {
            StopAttackImmediately();
        }
    }
}