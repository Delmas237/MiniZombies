using UnityEngine;

namespace PlayerLib
{
    public class PlayerContainer : MonoBehaviour, IPlayer
    {
        [Header("Modules")]
        [SerializeField] protected CurrencyModule _currencyModule;
        [SerializeField] protected HealthModule _healthModule;
        [SerializeField] protected PlayerWeaponsModule _weaponsModule;
        [SerializeField] protected PlayerMoveModule _moveModule;
        [SerializeField] protected PlayerAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;

        public Transform Transform => transform;
        public ICurrencyModule CurrencyModule => _currencyModule;
        public IHealthModule HealthModule => _healthModule;
        public IPlayerWeaponsModule WeaponsModule => _weaponsModule;
        public IPlayerMoveModule MoveModule => _moveModule;

        private void Awake()
        {
            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(HealthModule, WeaponsModule, MoveModule, GetComponent<Animator>());
            _weaponsModule.Initialize(HealthModule);
            _moveModule.Initialize(WeaponsModule, transform, GetComponent<Rigidbody>());

            _healthModule.IsOver += OnHealhIsOver;
        }
        private void OnHealhIsOver()
        {
            _moveModule.Rigidbody.velocity /= 2;
        }

        private void Update()
        {
            _moveModule.Move();
            _animationModule.MoveAnim();

            _moveModule.Rotate();
            _weaponsModule.UpdateShootLine();
        }

        private void OnDestroy()
        {
            _healthModule.IsOver -= OnHealhIsOver;
            _moveModule.OnDestroy();
        }
    }
}
