using EventBusLib;
using JoystickLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerLib
{
    [Serializable]
    public class PlayerMoveModule : IPlayerMoveModule
    {
        [SerializeField] private float _defaultSpeed = 3.65f;

        [Header("Rotation Smoothness")]
        [SerializeField] private float _rotationSmoothness = 13f;
        [SerializeField] private float _autoRotationSmoothness = 12f;
        [Space(10)]
        [SerializeField] private float _timeToUpdateClosestEnemy = 0.35f;
        [SerializeField] private Joystick _moveJoystick;

        private bool _controllable = true;
        private Rigidbody _rigidbody;
        private Transform _transform;

        private IEnemy _closestEnemy;
        private Coroutine _closestEnemyCoroutine;
        private IPlayerWeaponsModule _weaponsModule;

        public float DefaultSpeed => _defaultSpeed;
        public bool IsMoving => MoveJoystick.Direction != Vector2.zero;
        public bool IsTraking => _closestEnemy != null || _weaponsModule.AttackJoystick.Pressed;
        public Joystick MoveJoystick => _moveJoystick;
        public Rigidbody Rigidbody => _rigidbody;

        public void Initialize(IPlayerWeaponsModule weaponsModule, Transform transform, Rigidbody rigidbody)
        {
            _weaponsModule = weaponsModule;
            _transform = transform;
            _rigidbody = rigidbody;

            EventBus.Subscribe<GameOverEvent>(SetControllableFalse);
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);

            _closestEnemyCoroutine = CoroutineHelper.StartRoutine(UpdateClosestEnemy());
        }
        private void Unsubscribe(GameExitEvent exitEvent)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            EventBus.Unsubscribe<GameOverEvent>(SetControllableFalse);
        }

        private void SetControllableFalse(IEvent e)
        {
            _controllable = false;
        }

        public void Move()
        {
            if (_controllable)
            {
                Vector2 moveInput = new Vector2(MoveJoystick.Horizontal, MoveJoystick.Vertical);

                if (moveInput.sqrMagnitude > 1f)
                    moveInput.Normalize();

                _rigidbody.velocity = new Vector3(moveInput.x * _defaultSpeed, _rigidbody.velocity.y, moveInput.y * _defaultSpeed);
            }
        }

        public void Rotate()
        {
            if (_controllable)
            {
                if (_weaponsModule.AttackJoystick.Direction != Vector2.zero)
                {
                    RotateToJoystickDir(_weaponsModule.AttackJoystick, _rotationSmoothness * Time.deltaTime);
                }
                else if (_weaponsModule.AttackJoystick.UnPressedOrInDeadZoneTime > 0.15f)
                {
                    if (_closestEnemy != null)
                    {
                        RotateToClosestEnemy(_closestEnemy.Transform.position);
                    }
                    else if (MoveJoystick.Direction != Vector2.zero && _weaponsModule.AttackJoystick.UnPressedOrInDeadZoneTime > 0.05f)
                    {
                        RotateToJoystickDir(MoveJoystick, _rotationSmoothness * Time.deltaTime);
                    }
                }
            }
        }

        private void RotateToClosestEnemy(Vector3 closestEnemy)
        {
            closestEnemy -= _transform.position;
            closestEnemy = new Vector3(closestEnemy.x, 0, closestEnemy.z);

            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, Quaternion.LookRotation(closestEnemy), _autoRotationSmoothness * Time.deltaTime);
        }

        private void RotateToJoystickDir(Joystick joystick, float rotationSmoothness)
        {
            Vector3 moveDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            
            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, Quaternion.LookRotation(moveDir), rotationSmoothness);
        }

        private IEnumerator UpdateClosestEnemy()
        {
            while (true)
            {
                IReadOnlyList<IEnemy> closestEnemies = Spawner<IEnemy>.ObjectsOnScene;

                bool enemyInRange = false;
                IEnemy closestEnemy = null;
                if (closestEnemies.Count > 0)
                {
                    Vector3 position = _weaponsModule.CurrentGun.transform.position;
                    List<Transform> enemiesTransform = closestEnemies.Select(e => e.Transform).ToList();
                    Transform transform = ComponentSearcher<Transform>.Closest(position, enemiesTransform);
                    closestEnemy = closestEnemies[enemiesTransform.IndexOf(transform)];

                    float distanceToEnemy = Vector3.Distance(position, closestEnemy.Transform.position);
                    enemyInRange = distanceToEnemy <= _weaponsModule.CurrentGun.Distance;
                }
                _closestEnemy = enemyInRange ? closestEnemy : null;

                yield return new WaitForSeconds(_timeToUpdateClosestEnemy);
            }
        }

        public void OnDestroy()
        {
            CoroutineHelper.StopRoutine(_closestEnemyCoroutine);
        }
    }
}
