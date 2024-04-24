using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public abstract class Enemy : MonoBehaviour, IEnemy
    {
        public NavMeshAgent Agent { get; set; }

        [field: SerializeField] public HealthController HealthController { get; set; }
        [field: SerializeField] public EnemyAttackController AttackController { get; set; }
        [field: SerializeField] public EnemyMoveController MoveController { get; set; }
        [field: SerializeField] public EnemyAnimationController AnimationController { get; set; }

        protected virtual void Start()
        {
            HealthController.Initialize();
            AnimationController.Initialize(this);
            AttackController.Initialize(this);
            MoveController.Initialize(this);
        }

        protected virtual void Update()
        {
            MoveController.Move();

            AnimationController.MoveAnim();
            AnimationController.AttackAnim();
        }

        private void DealDamage() => AttackController.DealDamage();
    }
}
