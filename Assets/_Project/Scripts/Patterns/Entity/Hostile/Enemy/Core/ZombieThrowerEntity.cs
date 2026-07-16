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

        protected override void OnAwake()
        {
            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(GetComponent<Animator>(), HealthModule, TargetModule, MovementModule, AttackModule);
            _attackModule.Initialize(transform, TargetModule, MovementModule);
            _moveModule.Initialize(transform, GetComponent<NavMeshAgent>(), TargetModule, AttackModule);
            _deathModule.Initialize(this, HealthModule, MovementModule, AttackModule);

            _delayedDisableModule.Initialize(gameObject, HealthModule);
            _dropAmmoAfterDeathModule.Initialize(transform, HealthModule);
        }

        private void Shoot() => ThrowerAttackModule.Throw();
    }
}
