using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieTankContainer : ZombieContainer
    {
        [field: Space(10)]
        [field: Header("Controllers")]
        [field: SerializeField] public EnemyAttackController AttackController { get; set; }

        protected override void Start()
        {
            HealthController.Initialize();
            AnimationController.Initialize(HealthController, MoveController, AttackController, GetComponent<Animator>());
            AttackController.Initialize(HealthController, MoveController);
            MoveController.Initialize(GetComponent<NavMeshAgent>());

            DropAmmoAfterDeathModule.Initialize(HealthController, transform);
        }

        protected virtual void Update()
        {
            MoveController.Move();
            AnimationController.MoveAnim();

            AnimationController.AttackAnim();
        }

        private void OnCollisionEnter(Collision collision)
        {
            AttackController.OnCollisionEnter(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            AttackController.OnCollisionExit(collision);
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            MoveController.Agent.enabled = false;
            AttackController.IsAttack = false;
            enabled = false;
        }

        private void DealDamage() => AttackController.DealDamage();
    }
}
