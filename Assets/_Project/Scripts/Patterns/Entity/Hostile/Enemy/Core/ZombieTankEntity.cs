using UnityEngine;
using UnityEngine.AI;

namespace Entity.Hostile
{
    public class ZombieTankEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyMovementModule _moveModule;
        [SerializeField] protected ZombieTankAttackModule _attackModule;
        [SerializeField] protected EnemyDeathModule _deathModule;

        public override IEnemyMovementModule MovementModule => _moveModule;
        public override IEnemyAttackModule AttackModule => _attackModule;

        protected void Awake()
        {
            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(GetComponent<Animator>(), HealthModule, TargetModule, MovementModule, AttackModule);
            _attackModule.Initialize(TargetModule, MovementModule);
            _moveModule.Initialize(transform, GetComponent<NavMeshAgent>(), TargetModule, AttackModule);
            _deathModule.Initialize(this, HealthModule, MovementModule, AttackModule);

            _delayedDisableModule.Initialize(gameObject, _healthModule);
            _dropAmmoAfterDeathModule.Initialize(transform, HealthModule);
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

        protected void OnDestroy()
        {
            _attackModule.Dispose();
            _animationModule.Dispose();
            _audioModule.Dispose();
            _deathModule.Dispose();
        }

        private void DealDamage() => _attackModule.DealDamage();
    }
}
