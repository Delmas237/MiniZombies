using Entity;
using UnityEngine;

namespace Player
{
    public class PlayerEntity : MonoBehaviour, IPlayer
    {
        [Header("Modules")]
        [SerializeField] protected EntityHealthModule _healthModule;
        [SerializeField] protected PlayerCurrencyModule _currencyModule;
        [Space(10)]
        [SerializeField] protected PlayerInputModule _inputModule;
        [SerializeField] protected PlayerWeaponsModule _weaponsModule;
        [SerializeField] protected PlayerMovementModule _moveModule;
        [Space(10)]
        [SerializeField] protected PlayerAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;
        [SerializeField] protected PlayerShootLineModule _shootLineModule;

        public Transform Transform => transform;
        public IPlayerCurrencyModule CurrencyModule => _currencyModule;
        public IEntityHealthModule HealthModule => _healthModule;
        public IPlayerInputModule InputModule => _inputModule;
        public IPlayerWeaponModule WeaponModule => _weaponsModule;
        public IPlayerMovementModule MovementModule => _moveModule;

        private void Awake()
        {
            _shootLineModule.Initialize(WeaponModule);

            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);

            _inputModule.Initialize(transform, HealthModule, MovementModule, WeaponModule, _shootLineModule);
            _animationModule.Initialize(GetComponent<Animator>(), HealthModule, InputModule, WeaponModule, MovementModule);

            _weaponsModule.Initialize();
            _moveModule.Initialize(transform, GetComponent<Rigidbody>(), WeaponModule);

            _healthModule.IsOver += OnHealhIsOver;
        }
        private void OnHealhIsOver()
        {
            _moveModule.Rigidbody.linearVelocity /= 2;
        }

        private void Update()
        {
            _inputModule.Update();
            _animationModule.MoveAnim();
        }

        private void OnDestroy()
        {
            _healthModule.IsOver -= OnHealhIsOver;
            _inputModule.OnDestroy();
            _moveModule.OnDestroy();
        }
    }
}
