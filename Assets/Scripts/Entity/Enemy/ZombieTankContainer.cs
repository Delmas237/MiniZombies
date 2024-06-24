using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieTankContainer : ZombieContainer
    {
        [Space(10), Header("Controllers")]
        [SerializeField] protected ZombieTankAttackController _attackController;
        public override IEnemyAttackController AttackController => _attackController;

        protected override void Awake()
        {
            base.Awake();

            _healthController.Initialize();
            _animationController.Initialize(HealthController, MoveController, _attackController, GetComponent<Animator>());
            _attackController.Initialize(HealthController, MoveController);
            _moveController.Initialize(GetComponent<NavMeshAgent>(), transform, _attackController);

            DelayedDisableModule.Initialize(gameObject, this);
            DropAmmoAfterDeathModule.Initialize(HealthController, transform);
        }

        protected virtual void Update()
        {
            _moveController.Move();
            _animationController.MoveAnim();

            _animationController.AttackAnim();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _attackController.OnCollisionEnter(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            _attackController.OnCollisionExit(collision);
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            _moveController.Agent.enabled = false;
            _attackController.IsAttack = false;
            enabled = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _animationController.OnDestroy();
        }

        private void DealDamage() => _attackController.DealDamage();
    }
}
