using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieTankEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyMovementModule _moveModule;
        [SerializeField] protected ZombieTankAttackModule _attackModule;

        public override IEnemyMovementModule MovementModule => _moveModule;
        public override IEnemyAttackModule AttackModule => _attackModule;

        protected override void Awake()
        {
            base.Awake();

            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(HealthModule, MovementModule, AttackModule, GetComponent<Animator>());
            _attackModule.Initialize(HealthModule, MovementModule);
            _moveModule.Initialize(GetComponent<NavMeshAgent>(), transform, AttackModule);

            _delayedDisableModule.Initialize(gameObject, this);
            _dropAmmoAfterDeathModule.Initialize(HealthModule, transform);
        }

        protected override void OnEnable()
        {
            _moveModule.UpdateData();
            base.OnEnable();
        }

        protected virtual void Update()
        {
            _moveModule.Move();
            _animationModule.MoveAnim();
            _moveModule.Rotate();

            _animationModule.AttackAnim();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _attackModule.OnCollisionEnter(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            _attackModule.OnCollisionExit(collision);
        }

        protected override void OnHealhIsOver()
        {
            base.OnHealhIsOver();

            _moveModule.Agent.enabled = false;
            _attackModule.IsAttack = false;
            enabled = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _animationModule.OnDestroy();
        }

        private void DealDamage() => _attackModule.DealDamage();
    }
}
