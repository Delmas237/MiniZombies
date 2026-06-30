using UnityEngine;

namespace Entity.Friendly.Player
{
    public class PlayerEntity : MonoBehaviour, IPlayer
    {
        [Header("Modules")]
        [SerializeField] protected EntityHealthModule _healthModule;
        [SerializeField] protected PlayerCurrencyModule _currencyModule;
        [Space(10)]
        [SerializeField] protected PlayerInputModule _inputModule;
        [SerializeField] protected PlayerMovementModule _moveModule;
        [SerializeField] protected FriendlyTargetModule _targetModule;
        [SerializeField] protected PlayerWeaponModule _weaponsModule;
        [Space(10)]
        [SerializeField] protected PlayerAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;
        [SerializeField] protected PlayerShootLineModule _shootLineModule;
        [SerializeField] protected PlayerDeathModule _deathModule;

        public Transform Transform => transform;
        public IPlayerCurrencyModule CurrencyModule => _currencyModule;
        public IEntityHealthModule HealthModule => _healthModule;
        public IPlayerInputModule InputModule => _inputModule;
        public IPlayerMovementModule MovementModule => _moveModule;
        public IEntityTargetModule TargetingModule => _targetModule;
        public IPlayerWeaponModule WeaponModule => _weaponsModule;

        private void Awake()
        {
            _shootLineModule.Initialize(WeaponModule);

            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);

            _inputModule.Initialize(transform, HealthModule, MovementModule, TargetingModule, WeaponModule, _shootLineModule);
            _animationModule.Initialize(GetComponent<Animator>(), HealthModule, InputModule, WeaponModule);

            _weaponsModule.Initialize();
            _moveModule.Initialize(transform, GetComponent<Rigidbody>());
            _targetModule.Initialize(WeaponModule);
            _deathModule.Initialize(_healthModule, _moveModule);
        }

        private void Update()
        {
            _inputModule.Update();
            _animationModule.MoveAnim();
        }

        private void OnDestroy()
        {
            _inputModule.Dispose();
            _targetModule.Dispose();
            _animationModule.Dispose();
            _audioModule.Dispose();
            _deathModule.Dispose();
        }
    }
}
