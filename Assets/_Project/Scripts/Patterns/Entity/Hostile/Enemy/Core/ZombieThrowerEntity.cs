using UnityEngine;
using UnityEngine.AI;

namespace Entity.Hostile
{
    public class ZombieThrowerEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyAvoidantMovementModule _moveModule;
        [SerializeField] protected ZombieThrowerAttackModule _attackModule;
        [SerializeField] protected EnemyDeathModule _deathModule;

        public IEnemyThrowerAttackModule ThrowerAttackModule => _attackModule;
        public override IEnemyMovementModule MovementModule => _moveModule;
        public override IEnemyAttackModule AttackModule => _attackModule;

        protected void Awake()
        {
            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(GetComponent<Animator>(), HealthModule, TargetModule, MovementModule, AttackModule);
            _attackModule.Initialize(transform, TargetModule, MovementModule);
            _moveModule.Initialize(transform, GetComponent<NavMeshAgent>(), TargetModule, AttackModule);
            _deathModule.Initialize(this, HealthModule, MovementModule, AttackModule);

            _delayedDisableModule.Initialize(gameObject, _healthModule);
            _dropAmmoAfterDeathModule.Initialize(transform, HealthModule);
        }

        protected override void OnEnable()
        {
            _attackModule.UpdateData();
            _moveModule.UpdateData();
            base.OnEnable();
        }

        protected virtual void Update()
        {
            _moveModule.Move();
            _animationModule.MoveAnim();
            _moveModule.Rotate();

            _attackModule.UpdateState();
            _animationModule.AttackAnim();
        }

        protected void OnDestroy()
        {
            _attackModule.Dispose();
            _animationModule.Dispose();
            _audioModule.Dispose();
            _deathModule.Dispose();
        }

        private void Shoot() => ThrowerAttackModule.Throw();
    }
}
