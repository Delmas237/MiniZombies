using Entity;
using Entity.Hostile;
using EventBusLib;
using JoystickLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerMovementModule : IPlayerMovementModule
    {
        [SerializeField] private float _defaultSpeed = 3.65f;

        [Header("Rotation Smoothness")]
        [SerializeField] private float _rotationSpeed = 13f;
        [SerializeField] private float _autoRotationSpeed = 12f;
        [Space(10)]
        [SerializeField] private float _timeToUpdateClosestEnemy = 0.35f;
        [SerializeField] private Joystick _moveJoystick;

        private bool _controllable = true;
        private Rigidbody _rigidbody;

        private IHostile _closestEnemy;
        private Coroutine _closestEnemyCoroutine;
        private IEntityWeaponModule _weaponModule;

        public float DefaultSpeed => _defaultSpeed;
        public bool IsMoving => MoveJoystick.Direction != Vector2.zero;
        public IHostile ClosestEnemy => _closestEnemy;
        public bool IsTraking => _closestEnemy != null || _weaponModule.AttackJoystick.Pressed;
        public Joystick MoveJoystick => _moveJoystick;
        public Rigidbody Rigidbody => _rigidbody;

        public void Initialize(IEntityWeaponModule weaponModule, Rigidbody rigidbody)
        {
            _weaponModule = weaponModule;
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

                _rigidbody.linearVelocity = new Vector3(moveInput.x * _defaultSpeed, _rigidbody.linearVelocity.y, moveInput.y * _defaultSpeed);
            }
        }

        public void RotateToClosestEnemy(Vector3 closestEnemy)
        {
            if (!_controllable)
                return;

            closestEnemy -= _transform.position;
            closestEnemy = new Vector3(closestEnemy.x, 0, closestEnemy.z);

            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, Quaternion.LookRotation(closestEnemy), _autoRotationSpeed * Time.deltaTime);
        }

        public void RotateToDirection(Vector2 direction)
        {
            if (!_controllable)
                return;

            Vector3 rotateDirection = new Vector3(direction.x, 0, direction.y);
            
            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, Quaternion.LookRotation(rotateDirection), _rotationSpeed * Time.deltaTime);
        }

        private IEnumerator UpdateClosestEnemy()
        {
            while (true)
            {
                IReadOnlyList<IHostile> closestEnemies = Spawner<IHostile>.ObjectsOnScene;

                bool enemyInRange = false;
                IHostile closestEnemy = null;
                if (closestEnemies.Count > 0)
                {
                    Vector3 position = _weaponModule.CurrentGun.transform.position;
                    List<Transform> enemiesTransform = closestEnemies.Select(e => e.Transform).ToList();
                    Transform transform = ComponentSearcher<Transform>.Closest(position, enemiesTransform);
                    closestEnemy = closestEnemies[enemiesTransform.IndexOf(transform)];

                    float distanceToEnemy = Vector3.Distance(position, closestEnemy.Transform.position);
                    enemyInRange = distanceToEnemy <= _weaponModule.CurrentGun.Distance;
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
