using UnityEngine;
using UnityEngine.AI;

namespace Entity.Hostile
{
    public class ZombieShooterEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyMovementModule _moveModule;
        [SerializeField] protected ZombieShooterAttackModule _attackModule;
        [SerializeField] protected EntityWeaponModule _weaponsModule;
        [SerializeField] protected EnemyDeathModule _deathModule;

        public override IEnemyMovementModule MovementModule => _moveModule;
        public override IEnemyAttackModule AttackModule => _attackModule;
        public IEntityWeaponModule WeaponModule => _weaponsModule;

        protected void Awake()
        {
            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(GetComponent<Animator>(), HealthModule, TargetModule, MovementModule, AttackModule);
            _weaponsModule.Initialize();
            _attackModule.Initialize(transform, TargetModule, MovementModule, WeaponModule);
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

        private void Shoot() => _weaponsModule.PullTrigger();
    }
}
