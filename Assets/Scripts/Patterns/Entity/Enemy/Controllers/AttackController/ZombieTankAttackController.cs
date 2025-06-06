using System;
using System.Collections;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class ZombieTankAttackController : IEnemyAttackController
    {
        [SerializeField, Range(0.01f, 3f)] protected float _defaultSpeed = 1f;
        [SerializeField] private int _damage = 15;

        [Space(10), Tooltip("Attack stopping speed divided by attack speed")]
        [SerializeField, Range(0, 3f)] private float _stopAttackSpeedRatio = 0.3f;

        private IEntity _targetCollision;
        private IHealthController _healthController;
        private IEnemyMoveController _moveController;

        public bool IsAttack { get; set; }
        public float Speed { get; set; }

        public float DefaultSpeed => _defaultSpeed;
        public int Damage => _damage;

        public void Initialize(IHealthController healthController, IEnemyMoveController moveController)
        {
            _healthController = healthController;
            _moveController = moveController;
            Speed = DefaultSpeed;
        }

        private void Attack()
        {
            _moveController.Agent.isStopped = true;
            IsAttack = true;
        }
        private IEnumerator StopAttack(float delay)
        {
            yield return new WaitForSeconds(delay);
            _targetCollision = null;

            if (_moveController.Agent != null && _moveController.Agent.enabled)
                _moveController.Agent.isStopped = false;

            IsAttack = false;
        }

        public void DealDamage()
        {
            if (_targetCollision == null || _targetCollision.HealthController.Health <= 0)
            {
                CoroutineHelper.StartRoutine(StopAttack(0));
                return;
            }

            _targetCollision.HealthController.Decrease(Damage);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IEntity entity) && entity == _moveController.Target && 
                entity.HealthController.Health > 0 && _healthController.Health > 0)
            {
                _targetCollision = entity;
                Attack();
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IEntity entity) && entity == _moveController.Target)
            {
                CoroutineHelper.StartRoutine(StopAttack(_stopAttackSpeedRatio / Speed));
            }
        }
    }
}
