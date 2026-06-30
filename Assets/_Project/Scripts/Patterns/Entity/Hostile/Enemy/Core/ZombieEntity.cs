using UnityEngine;

namespace Entity.Hostile
{
    public abstract class ZombieEntity : EntityBase, IHostile
    {
        [Header("Base Modules")]
        [SerializeField] protected EntityHealthModule _healthModule;
        [SerializeField] protected EnemyTargetModule _targetModule;
        [SerializeField] protected EnemyAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;
        [Space(10)]
        [SerializeField] protected EntityDelayedDisableModule _delayedDisableModule;
        [SerializeField] protected EntityDropAmmoOnDeathModule _dropAmmoAfterDeathModule;

        public override IEntityHealthModule HealthModule => _healthModule;
        public IEntityTargetModule TargetModule => _targetModule;
        public EntityDelayedDisableModule DelayedDisableModule => _delayedDisableModule;
        public EntityDropAmmoOnDeathModule DropAmmoAfterDeathModule => _dropAmmoAfterDeathModule;

        public abstract IEnemyMovementModule MovementModule { get; }
        public abstract IEnemyAttackModule AttackModule { get; }

        protected virtual void OnEnable()
        {
            _animationModule.UpdateData();
        }
    }
}
