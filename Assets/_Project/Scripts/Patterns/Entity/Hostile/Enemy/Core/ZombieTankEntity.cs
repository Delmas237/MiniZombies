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

        protected override void OnAwake()
        {
            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(GetComponent<Animator>(), HealthModule, TargetModule, MovementModule, AttackModule);
            _attackModule.Initialize(TargetModule, MovementModule);
            _moveModule.Initialize(transform, GetComponent<NavMeshAgent>(), TargetModule, AttackModule);
            _deathModule.Initialize(this, HealthModule, MovementModule, AttackModule);

            _delayedDisableModule.Initialize(gameObject, HealthModule);
            _dropAmmoAfterDeathModule.Initialize(transform, HealthModule);
        }

        private void DealDamage() => _attackModule.DealDamage();
    }
}
