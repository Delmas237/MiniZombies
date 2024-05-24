using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieTankContainer : ZombieContainer
    {
        [field: Space(10)]
        [field: Header("Controllers")]
        [field: SerializeField] public ZombieTankAttackController AttackController { get; set; }

        protected override void Start()
        {
            base.Start();

            _healthController.Initialize();
            _animationController.Initialize(HealthController, MoveController, AttackController, GetComponent<Animator>());
            AttackController.Initialize(HealthController, MoveController);
            _moveController.Initialize(GetComponent<NavMeshAgent>(), transform, AttackController);

            DropAmmoAfterDeathModule.Initialize(HealthController, transform);
        }

        private void OnEnable()
        {
            _moveController.OnEnable();
        }

        protected virtual void Update()
        {
            _moveController.Move();
            _animationController.MoveAnim();

            _animationController.AttackAnim();
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

            _moveController.Agent.enabled = false;
            AttackController.IsAttack = false;
            enabled = false;
        }

        private void DealDamage() => AttackController.DealDamage();
    }
}
