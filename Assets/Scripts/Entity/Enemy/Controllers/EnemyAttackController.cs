using System;
using System.Collections;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class EnemyAttackController
    {
        public bool IsAttack { get; set; }
        private IPlayer _targetCollision;

        [SerializeField] private int _damage = 15;
        public int Damage => _damage;

        private IHealthController _healthController;
        private IEnemyMoveController _moveController;

        public void Initialize(IHealthController healthController, IEnemyMoveController moveController)
        {
            _healthController = healthController;
            _moveController = moveController;
        }

        protected void Attack()
        {
            _moveController.Agent.isStopped = true;
            IsAttack = true;
        }

        public void OnCollisionEnter(Collision collision)
        {
            IPlayer target = collision.gameObject.GetComponent<IPlayer>();

            if (target != null && target.HealthController.Health > 0 && _healthController.Health > 0)
            {
                _targetCollision = target;
                Attack();
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent<IPlayer>() != null)
            {
                CoroutineHelper.StartRoutine(StopAttack(0.3f));
            }
        }

        private IEnumerator StopAttack(float delay)
        {
            yield return new WaitForSeconds(delay);
            _targetCollision = null;

            if (_moveController.Agent.enabled)
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

            _targetCollision.HealthController.Health -= Damage;
        }
    }
}
