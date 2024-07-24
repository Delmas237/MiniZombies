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
    public class PlayerMoveController : IPlayerMoveController
    {
        private bool _controllable = true;

        [SerializeField] private float _defaultSpeed = 3.65f;
        public float DefaultSpeed => _defaultSpeed;
        public bool IsMoving => MoveJoystick.Horizontal != 0 || MoveJoystick.Vertical != 0;

        [field: SerializeField] public Joystick MoveJoystick { get; private set; }

        [Header("Rotation Smoothness")]
        [SerializeField] private float _rotationSmoothness = 13f;
        [SerializeField] private float _autoRotationSmoothness = 12f;

        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody;

        private Transform _transform;

        private IPlayerWeaponsController _weaponsController;

        private IEnemy _closestEnemy;
        private const float TIME_TO_UPDATE_CLOSEST_ENEMY = 0.35f;
        private Coroutine _closestEnemyCoroutine;

        public void Initialize(IPlayerWeaponsController weaponsController, Transform transform, Rigidbody rigidbody)
        {
            _weaponsController = weaponsController;
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
                float speedFactor = (MoveJoystick.Horizontal == 0 || MoveJoystick.Vertical == 0) ? 1 : 1.5f;

                _rigidbody.velocity = new Vector3(MoveJoystick.Horizontal * _defaultSpeed / speedFactor, _rigidbody.velocity.y, 
                    MoveJoystick.Vertical * _defaultSpeed / speedFactor);
            }
        }

        public void Rotate()
        {
            if (_controllable)
            {
                if (_weaponsController.AttackJoystick.Direction != Vector2.zero)
                {
                    RotateToJoystickDir(_weaponsController.AttackJoystick, _rotationSmoothness * Time.deltaTime);
                }
                else if (_weaponsController.AttackJoystick.UnPressedOrInDeadZoneTime > 0.15f)
                {
                    if (_closestEnemy != null)
                    {
                        RotateToClosestEnemy(_closestEnemy.Transform.position);
                    }
                    else if (MoveJoystick.Direction != Vector2.zero && _weaponsController.AttackJoystick.UnPressedOrInDeadZoneTime > 0.05f)
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
                    Vector3 position = _weaponsController.CurrentGun.transform.position;
                    List<Transform> enemiesTransform = closestEnemies.Select(e => e.Transform).ToList();
                    Transform transform = ComponentSearcher<Transform>.Closest(position, enemiesTransform);
                    closestEnemy = closestEnemies[enemiesTransform.IndexOf(transform)];

                    float distanceToEnemy = Vector3.Distance(position, closestEnemy.Transform.position);
                    enemyInRange = distanceToEnemy <= _weaponsController.CurrentGun.Distance;
                }
                _closestEnemy = enemyInRange ? closestEnemy : null;

                yield return new WaitForSeconds(TIME_TO_UPDATE_CLOSEST_ENEMY);
            }
        }

        public void OnDestroy()
        {
            CoroutineHelper.StopRoutine(_closestEnemyCoroutine);
        }
    }
}
