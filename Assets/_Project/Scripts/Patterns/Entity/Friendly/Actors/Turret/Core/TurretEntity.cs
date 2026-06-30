using UnityEngine;

namespace Entity.Friendly.Turret
{
    public class TurretEntity : EntityBase, IFriendly
    {
        [Header("Modules")]
        [SerializeField] protected EntityHealthModule _healthModule;
        [Space(10)]
        [SerializeField] protected FriendlyTargetModule _targetModule;
        [SerializeField] protected EntityWeaponModule _weaponsModule;
        [SerializeField] protected TurretAttackModule _attackModule;
        [SerializeField] protected TurretRotationModule _rotationModule;
        [Space(10)]
        [SerializeField] protected TurretAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;
        [SerializeField] protected TurretDeathModule _deathModule;

        public override IEntityHealthModule HealthModule => _healthModule;
        public IEntityTargetModule TargetModule => _targetModule;
        public IEntityWeaponModule WeaponModule => _weaponsModule;
        public TurretAttackModule AttackModule => _attackModule;

        protected override void OnAwake()
        {
            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);

            _weaponsModule.Initialize();
            _targetModule.Initialize(WeaponModule);
            _attackModule.Initialize(TargetModule, WeaponModule);
            _rotationModule.Initialize(TargetModule);

            _animationModule.Initialize(HealthModule, TargetModule, AttackModule);
            _deathModule.Initialize(_healthModule, _attackModule);
        }

        private void Update()
        {
            _attackModule.Attack();
            _rotationModule.Rotate();
            _animationModule.UpdateState();
        }
    }
}
