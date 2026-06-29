using System;
using UnityEngine;
using Weapons;

namespace Entity.Friendly.Player
{
    [Serializable]
    public class PlayerInputModule : IPlayerInputModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private PlayerMobileInput _mobileInput;

        private Transform _transform;
        private IEntityHealthModule _healthModule;
        private IPlayerMovementModule _movementModule;
        private IEntityTargetModule _targetingModule;
        private IPlayerWeaponModule _weaponModule;
        private PlayerShootLineModule _shootLineModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public bool HasMoveInput => _enabled && _mobileInput.MoveJoystick.Direction != Vector2.zero;
        public bool IsTraking => _enabled && (_targetingModule.Target != null || _mobileInput.AttackJoystick.Pressed);

        public void Initialize(Transform transform, IEntityHealthModule healthModule, IPlayerMovementModule movementModule, IEntityTargetModule targetModule,
            IPlayerWeaponModule weaponModule, PlayerShootLineModule shootLineModule)
        {
            _transform = transform;
            _healthModule = healthModule;
            _movementModule = movementModule;
            _targetingModule = targetModule;
            _weaponModule = weaponModule;
            _shootLineModule = shootLineModule;

            _healthModule.IsOver += Unsubscribe;
            _weaponModule.GunChanged += UpdateShootLineScale;

            _mobileInput.AttackJoystick.OnUp += OnAttackUp;
            _mobileInput.AttackJoystick.OnClamped += OnAttackClamped;
        }
        private void UpdateShootLineScale(Gun gun)
        {
            if (!_enabled)
                return;

            _shootLineModule.UpdateShootLineScale();
        }
        private void OnAttackUp()
        {
            if (!_enabled)
                return;

            _weaponModule.PullTrigger();
        }
        private void OnAttackClamped()
        {
            if (!_enabled)
                return;

            _weaponModule.PullAutoTrigger();
        }

        public void Update()
        {
            if (!_enabled)
                return;

            Move();
            Rotate();
            UpdateShootLine();
        }

        private void Move()
        {
            _movementModule.Move(_mobileInput.MoveJoystick.Direction);
        }
        private void Rotate()
        {
            Vector3 direction;
            if (_mobileInput.AttackJoystick.Direction != Vector2.zero)
            {
                direction = new Vector3(_mobileInput.AttackJoystick.Direction.x, 0, _mobileInput.AttackJoystick.Direction.y);
                _movementModule.RotateToDirection(direction);
            }
            else if (_mobileInput.AttackJoystick.UnPressedOrInDeadZoneTime > 0.15f)
            {
                if (_targetingModule.Target != null)
                {
                    direction = _targetingModule.Target.Transform.position;
                    direction -= _transform.position;
                    direction.y = 0;
                    _movementModule.RotateToDirection(direction);
                }
                else if (_mobileInput.MoveJoystick.Direction != Vector2.zero && _mobileInput.AttackJoystick.UnPressedOrInDeadZoneTime > 0.05f)
                {
                    direction = new Vector3(_mobileInput.MoveJoystick.Direction.x, 0, _mobileInput.MoveJoystick.Direction.y);
                    _movementModule.RotateToDirection(direction);
                }
            }
        }
        private void UpdateShootLine()
        {
            _shootLineModule.UpdateShootLine(_mobileInput.AttackJoystick.Direction);
        }

        public void Dispose()
        {
            Unsubscribe();
        }
        private void Unsubscribe()
        {
            _healthModule.IsOver -= Unsubscribe;
            _weaponModule.GunChanged -= UpdateShootLineScale;

            _mobileInput.AttackJoystick.OnUp -= OnAttackUp;
            _mobileInput.AttackJoystick.OnClamped -= OnAttackClamped;
        }
    }
}
