using UnityEngine;

namespace PlayerLib
{
    public class PlayerContainer : MonoBehaviour, IPlayer
    {
        [Header("Controllers")]
        [SerializeField] protected CurrencyController _currencyController;
        public ICurrencyController CurrencyController => _currencyController;

        [SerializeField] protected HealthController _healthController;
        public IHealthController HealthController => _healthController;

        [SerializeField] protected PlayerWeaponsController _weaponsController;
        public IPlayerWeaponsController WeaponsController => _weaponsController;

        [SerializeField] protected PlayerMoveController _moveController;
        public IPlayerMoveController MoveController => _moveController;

        [SerializeField] protected PlayerAnimationController _animationController;

        public Transform Transform => transform;

        private void Awake()
        {
            _healthController.Initialize();
            _animationController.Initialize(HealthController, WeaponsController, MoveController, GetComponent<Animator>());
            _weaponsController.Initialize(HealthController, MoveController);
            _moveController.Initialize(HealthController, WeaponsController, transform, GetComponent<Rigidbody>());
        }

        private void Update()
        {
            _moveController.Move();
            _animationController.MoveAnim();

            _moveController.Rotation();
            _weaponsController.UpdateShootLine();
        }

        private void OnDestroy()
        {
            _moveController.OnDestroy();
        }
    }
}
