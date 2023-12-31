using PlayerLib;
using System.Collections;
using UnityEngine;

namespace EnemyLib
{
    public class EnemyAttackController : MonoBehaviour
    {
        public bool IsAttack { get; set; }
        private Player targetCollision;

        [SerializeField] private int damage = 15;
        public int Damage => damage;

        private Enemy enemy;

        public void Initialize(Enemy _enemy)
        {
            enemy = _enemy;
        }

        protected void Attack()
        {
            enemy.Agent.isStopped = true;
            IsAttack = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Player target = collision.gameObject.GetComponent<Player>();

            if (target != null && target.HealthController.Health > 0 && enemy.HealthController.Health > 0)
            {
                targetCollision = target;
                Attack();
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            Player target = collision.gameObject.GetComponent<Player>();

            if (target != null)
            {
                targetCollision = null;
                StartCoroutine(StopAttack(1));
            }
        }

        private IEnumerator StopAttack(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (enemy.Agent.enabled)
                enemy.Agent.isStopped = false;

            IsAttack = false;
        }

        //using in animation event
        private void DealDamage()
        {
            if (targetCollision != null && targetCollision.HealthController.Health > 0)
                targetCollision.HealthController.Health -= Damage;
            else
                StartCoroutine(StopAttack(0));
        }
    }
}
