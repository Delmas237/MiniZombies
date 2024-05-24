using EnemyLib;
using JoystickLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerLib
{
    [Serializable]
    public class PlayerMoveController : IPlayerMoveController
    {
        private bool _controllable = true;

        [SerializeField] private float _maxSpeed = 3.65f;
        public bool Walks => MoveJoystick.Horizontal != 0 || MoveJoystick.Vertical != 0;

        [field: SerializeField] public Joystick MoveJoystick { get; private set; }

        [Header("Rotation Smoothness")]
        [SerializeField] private float _rotationSmoothness = 0.25f;
        [SerializeField] private float _autoRotationSmoothness = 0.2f;

        private Rigidbody _rb;
        private Transform _transform;

        private IHealthController _healthController;
        private IPlayerWeaponsController _weaponsController;

        private ZombieContainer _closestEnemy;
        private const float TIME_TO_UPDATE_CLOSEST_ENEMY = 0.35f;
        private Coroutine _closestEnemyCoroutine;

        public void Initialize(IHealthController healthController, IPlayerWeaponsController weaponsController, Transform transform, 
            Rigidbody rigidbody)
        {
            _healthController = healthController;
            _weaponsController = weaponsController;
            _transform = transform;
            _rb = rigidbody;

            _healthController.Died += SetControllableFalse;

            _closestEnemyCoroutine = CoroutineHelper.StartRoutine(UpdateClosestEnemy());
        }
        private void SetControllableFalse() => _controllable = false;

        public void Move()
        {
            if (_controllable)
            {
                float speedFactor = (MoveJoystick.Horizontal == 0 || MoveJoystick.Vertical == 0) ? 1 : 1.5f;

                _rb.velocity = new Vector3(MoveJoystick.Horizontal * _maxSpeed / speedFactor, _rb.velocity.y, 
                    MoveJoystick.Vertical * _maxSpeed / speedFactor);
            }
        }

        public void Rotation()
        {
            if (_controllable)
            {
                if (_weaponsController.AttackJoystick.Direction != Vector2.zero)
                {
                    RotateToJoystickDir(_weaponsController.AttackJoystick, _rotationSmoothness);
                }
                else if (_closestEnemy && _weaponsController.AttackJoystick.UnPressedOrInDeadZoneTime > 0.15f)
                {
                    RotateToClosestEnemy(_closestEnemy.transform.position);
                }
                else if (MoveJoystick.Direction != Vector2.zero && _weaponsController.AttackJoystick.UnPressedOrInDeadZoneTime > 0.05f)
                {
                    RotateToJoystickDir(MoveJoystick, _rotationSmoothness);
                }
            }
        }

        private void RotateToClosestEnemy(Vector3 closestEnemy)
        {
            closestEnemy -= _transform.position;
            closestEnemy = new Vector3(closestEnemy.x, 0, closestEnemy.z);

            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, Quaternion.LookRotation(closestEnemy), _autoRotationSmoothness);
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
                ComponentSearcher<ZombieContainer>.InRadius(
                    _transform.position, _weaponsController.CurrentGun.Distance, out List<ZombieContainer> closestEnemies);

                bool enemyInRange = false;
                ZombieContainer closestEnemy = null;
                if (closestEnemies != null)
                {
                    closestEnemy = ComponentSearcher<ZombieContainer>.Closest(_transform.position, closestEnemies);

                    if (closestEnemy && closestEnemy.HealthController.Health > 0)
                    {
                        float distanceToEnemy = Vector3.Distance(_transform.position, closestEnemy.transform.position);
                        enemyInRange = distanceToEnemy <= _weaponsController.CurrentGun.Distance;
                    }
                }
                if (enemyInRange)
                    _closestEnemy = closestEnemy;
                else
                    _closestEnemy = null;

                yield return new WaitForSeconds(TIME_TO_UPDATE_CLOSEST_ENEMY);
            }
        }

        public void OnDestroy()
        {
            CoroutineHelper.StopRoutine(_closestEnemyCoroutine);
        }
    }
}
