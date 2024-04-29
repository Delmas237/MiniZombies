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
        private PlayerContainer targetCollision;

        [SerializeField] private int damage = 15;
        public int Damage => damage;

        private EnemyContainer enemy;

        public void Initialize(EnemyContainer _enemy)
        {
            enemy = _enemy;
        }

        protected void Attack()
        {
            enemy.MoveController.Agent.isStopped = true;
            IsAttack = true;
        }

        public void OnCollisionEnter(Collision collision)
        {
            PlayerContainer target = collision.gameObject.GetComponent<PlayerContainer>();

            if (target != null && target.HealthController.Health > 0 && enemy.HealthController.Health > 0)
            {
                targetCollision = target;
                Attack();
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            PlayerContainer target = collision.gameObject.GetComponent<PlayerContainer>();

            if (target != null)
            {
                targetCollision = null;
                enemy.StartCoroutine(StopAttack(1));
            }
        }

        private IEnumerator StopAttack(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (enemy.MoveController.Agent.enabled)
                enemy.MoveController.Agent.isStopped = false;

            IsAttack = false;
        }

        public void DealDamage()
        {
            if (targetCollision != null && targetCollision.HealthController.Health > 0)
                targetCollision.HealthController.Health -= Damage;
            else
                enemy.StartCoroutine(StopAttack(0));
        }
    }
}
