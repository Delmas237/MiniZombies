using System;
using UnityEngine;
using Weapons;

namespace EntityLib.Friendly.Player
{
    [Serializable]
    public class PlayerInputModule : IPlayerInputModule, IUpdatable, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private PlayerMobileInput _mobileInput;

        private Transform _transform;
        private IEntityHealthModule _healthModule;
        private IPlayerMovementModule _movementModule;
        private IEntityTargetModule _targetingModule;
        private IEntityWeaponModule _weaponModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public bool HasMoveInput => Enabled && _mobileInput.MoveJoystick.Direction != Vector2.zero;
        public bool IsTraking => Enabled && (_targetingModule.Target != null || _mobileInput.AttackJoystick.Pressed);
        public Vector2 MoveDirection => _mobileInput.MoveJoystick.Direction;
        public Vector2 AttackDirection => _mobileInput.AttackJoystick.Direction;

        [ModuleInject]
        private void Initialize(Transform transform, IEntityHealthModule healthModule, IPlayerMovementModule movementModule, IEntityTargetModule targetModule,
            IEntityWeaponModule weaponModule)
        {
            _transform = transform;
            _healthModule = healthModule;
            _movementModule = movementModule;
            _targetingModule = targetModule;
            _weaponModule = weaponModule;

            _healthModule.IsOver += Unsubscribe;

            _mobileInput.AttackJoystick.OnUp += OnAttackUp;
            _mobileInput.AttackJoystick.OnClamped += OnAttackClamped;
        }

        private void OnAttackUp()
        {
            if (!Enabled)
                return;

            _weaponModule.PullTrigger();
        }
        private void OnAttackClamped()
        {
            if (!Enabled)
                return;

            _weaponModule.PullAutoTrigger();
        }

        public void Update()
        {
            Move();
            Rotate();
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

        public void Dispose()
        {
            Unsubscribe();
        }
        private void Unsubscribe()
        {
            if (_healthModule != null)
                _healthModule.IsOver -= Unsubscribe;

            if (_mobileInput != null)
            {
                _mobileInput.AttackJoystick.OnUp -= OnAttackUp;
                _mobileInput.AttackJoystick.OnClamped -= OnAttackClamped;
            }
        }
    }
}
