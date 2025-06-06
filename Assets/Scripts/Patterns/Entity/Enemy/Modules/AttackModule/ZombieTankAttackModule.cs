using System;
using System.Collections;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class ZombieTankAttackModule : IEnemyAttackModule
    {
        [SerializeField, Range(0.01f, 3f)] protected float _defaultSpeed = 1f;
        [SerializeField] private int _damage = 15;

        [Space(10), Tooltip("Attack stopping speed divided by attack speed")]
        [SerializeField, Range(0, 3f)] private float _stopAttackSpeedRatio = 0.3f;

        private IEntity _targetCollision;
        private IHealthModule _healthModule;
        private IEnemyMoveModule _moveModule;

        public bool IsAttack { get; set; }
        public float Speed { get; set; }

        public float DefaultSpeed => _defaultSpeed;
        public int Damage => _damage;

        public void Initialize(IHealthModule healthModule, IEnemyMoveModule moveModule)
        {
            _healthModule = healthModule;
            _moveModule = moveModule;
            Speed = DefaultSpeed;
        }

        private void Attack()
        {
            _moveModule.Agent.isStopped = true;
            IsAttack = true;
        }
        private IEnumerator StopAttack(float delay)
        {
            yield return new WaitForSeconds(delay);
            _targetCollision = null;

            if (_moveModule.Agent != null && _moveModule.Agent.enabled)
                _moveModule.Agent.isStopped = false;

            IsAttack = false;
        }

        public void DealDamage()
        {
            if (_targetCollision == null || _targetCollision.HealthModule.Health <= 0)
            {
                CoroutineHelper.StartRoutine(StopAttack(0));
                return;
            }

            _targetCollision.HealthModule.Decrease(Damage);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IEntity entity) && entity == _moveModule.Target && 
                entity.HealthModule.Health > 0 && _healthModule.Health > 0)
            {
                _targetCollision = entity;
                Attack();
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IEntity entity) && entity == _moveModule.Target)
            {
                CoroutineHelper.StartRoutine(StopAttack(_stopAttackSpeedRatio / Speed));
            }
        }
    }
}
