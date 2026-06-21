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
        public IPlayerWeaponModule WeaponModule => _weaponsModule;
        public IPlayerMovementModule MovementModule => _moveModule;

        private void Awake()
        {
            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _inputModule.Initialize(HealthModule, MovementModule, WeaponModule, _shootLineModule);
            _animationModule.Initialize(HealthModule, WeaponModule, MovementModule, GetComponent<Animator>());
            _weaponsModule.Initialize();
            _moveModule.Initialize(WeaponModule, transform, GetComponent<Rigidbody>());
            _shootLineModule.Initialize(WeaponModule);

            _healthModule.IsOver += OnHealhIsOver;
        }
        private void OnHealhIsOver()
        {
            _moveModule.Rigidbody.linearVelocity /= 2;
        }

        private void Update()
        {
            _moveModule.Move();
            _animationModule.MoveAnim();

            _moveModule.Rotate();
        }

        private void OnDestroy()
        {
            _healthModule.IsOver -= OnHealhIsOver;
            _inputModule.OnDestroy();
            _moveModule.OnDestroy();
        }
    }
}
