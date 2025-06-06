using UnityEngine;

namespace PlayerLib
{
    public class PlayerContainer : MonoBehaviour, IPlayer
    {
        [Header("Controllers")]
        [SerializeField] protected CurrencyController _currencyController;
        [SerializeField] protected HealthController _healthController;
        [SerializeField] protected PlayerWeaponsController _weaponsController;
        [SerializeField] protected PlayerMoveController _moveController;
        [SerializeField] protected PlayerAnimationController _animationController;

        public Transform Transform => transform;
        public ICurrencyController CurrencyController => _currencyController;
        public IHealthController HealthController => _healthController;
        public IPlayerWeaponsController WeaponsController => _weaponsController;
        public IPlayerMoveController MoveController => _moveController;

        private void Awake()
        {
            _healthController.Initialize();
            _animationController.Initialize(HealthController, WeaponsController, MoveController, GetComponent<Animator>());
            _weaponsController.Initialize(HealthController);
            _moveController.Initialize(WeaponsController, transform, GetComponent<Rigidbody>());

            _healthController.IsOver += OnHealhIsOver;
        }
        private void OnHealhIsOver()
        {
            _moveController.Rigidbody.velocity /= 2;
        }

        private void Update()
        {
            _moveController.Move();
            _animationController.MoveAnim();

            _moveController.Rotate();
            _weaponsController.UpdateShootLine();
        }

        private void OnDestroy()
        {
            _healthController.IsOver -= OnHealhIsOver;
            _moveController.OnDestroy();
        }
    }
}
