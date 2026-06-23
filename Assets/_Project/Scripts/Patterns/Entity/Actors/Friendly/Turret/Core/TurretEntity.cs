using UnityEngine;

namespace Entity.Friendly.Turret
{
    public class TurretEntity : MonoBehaviour, IFriendly
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

        public Transform Transform => transform;
        public IEntityHealthModule HealthModule => _healthModule;
        public IEntityTargetModule TargetModule => _targetModule;
        public IEntityWeaponModule WeaponModule => _weaponsModule;
        public TurretAttackModule AttackModule => _attackModule;
        public TurretRotationModule RotationModule => _rotationModule;
        public TurretAnimationModule AnimationModule => _animationModule;

        private void Start()
        {
            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _weaponsModule.Initialize();
            _targetModule.Initialize(WeaponModule);
            _attackModule.Initialize(TargetModule, WeaponModule);
            _rotationModule.Initialize(TargetModule);
            _animationModule.Initialize(HealthModule, TargetModule, AttackModule);

            _healthModule.IsOver += OnHealhIsOver;
        }
        private void OnHealhIsOver()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            _attackModule.Attack();
            _rotationModule.Rotate();
            _animationModule.UpdateState();
        }

        private void OnDestroy()
        {
            _healthModule.IsOver -= OnHealhIsOver;
            _attackModule.OnDestroy();
            _animationModule.OnDestroy();
        }
    }
}
