using PlayerLib;
using System;
using System.Collections;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class EnemyAttackController
    {
        public bool IsAttack { get; set; }
        private IPlayer targetCollision;

        [SerializeField] private int damage = 15;
        public int Damage => damage;

        private HealthController healthController;
        private EnemyMoveController moveController;

        public void Initialize(HealthController _healthController, EnemyMoveController _moveController)
        {
            healthController = _healthController;
            moveController = _moveController;
        }

        protected void Attack()
        {
            moveController.Agent.isStopped = true;
            IsAttack = true;
        }

        public void OnCollisionEnter(Collision collision)
        {
            IPlayer target = collision.gameObject.GetComponent<IPlayer>();

            if (target != null && target.HealthController.Health > 0 && healthController.Health > 0)
            {
                targetCollision = target;
                Attack();
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IPlayer player))
            {
                CoroutineHelper.StartRoutine(StopAttack(0.3f));
            }
        }

        private IEnumerator StopAttack(float delay)
        {
            yield return new WaitForSeconds(delay);
            targetCollision = null;

            if (moveController.Agent.enabled)
                moveController.Agent.isStopped = false;

            IsAttack = false;
        }

        public void DealDamage()
        {
            if (targetCollision != null && targetCollision.HealthController.Health > 0)
                targetCollision.HealthController.Health -= Damage;
            else
                CoroutineHelper.StartRoutine(StopAttack(0));
        }
    }
}
