using EnemyLib;
using JoystickLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerLib
{
    [Serializable]
    public class PlayerMoveController
    {
        private bool controllable = true;
        private Transform transform;

        [SerializeField] private float maxSpeed = 3.65f;
        public bool Walks => MoveJoystick.Horizontal != 0 || MoveJoystick.Vertical != 0;

        [field: SerializeField] public Joystick MoveJoystick { get; private set; }

        [Header("Rotation Smoothness")]
        [SerializeField] private float rotationSmoothness = 0.25f;
        [SerializeField] private float autoRotationSmoothness = 0.2f;
        public bool AutoRotate { get; set; }

        private Rigidbody rb;

        private HealthController healthController;
        private PlayerWeaponsController weaponsController;

        private ZombieContainer closestEnemy;
        private const float timeToUpdateClosestEnemy = 0.35f;
        private Coroutine ClosestEnemyCoroutine;

        public void Initialize(HealthController _healthController, PlayerWeaponsController _weaponsController, Transform _transform, 
            Rigidbody _rigidbody)
        {
            healthController = _healthController;
            weaponsController = _weaponsController;
            transform = _transform;
            rb = _rigidbody;

            healthController.Died += SetControllableFalse;

            ClosestEnemyCoroutine = CoroutineHelper.StartRoutine(UpdateClosestEnemy());
        }
        private void SetControllableFalse() => controllable = false;

        public void Move()
        {
            if (controllable)
            {
                float speedFactor = (MoveJoystick.Horizontal == 0 || MoveJoystick.Vertical == 0) ? 1 : 1.5f;

                rb.velocity = new Vector3(MoveJoystick.Horizontal * maxSpeed / speedFactor, rb.velocity.y, 
                    MoveJoystick.Vertical * maxSpeed / speedFactor);
            }
        }

        public void Rotation()
        {
            if (controllable)
            {
                if (weaponsController.AttackJoystick.Direction != Vector2.zero)
                {
                    RotateToJoystickDir(weaponsController.AttackJoystick, rotationSmoothness);
                }
                else if (closestEnemy && weaponsController.AttackJoystick.UnPressedOrInDeadZoneTime > 0.15f)
                {
                    RotateToClosestEnemy(closestEnemy.transform.position);
                }
                else if (MoveJoystick.Direction != Vector2.zero && weaponsController.AttackJoystick.UnPressedOrInDeadZoneTime > 0.05f)
                {
                    RotateToJoystickDir(MoveJoystick, rotationSmoothness);
                }
            }
        }

        private void RotateToClosestEnemy(Vector3 closestEnemy)
        {
            closestEnemy -= transform.position;
            closestEnemy = new Vector3(closestEnemy.x, 0, closestEnemy.z);

            transform.rotation = Quaternion.Lerp(
                transform.rotation, Quaternion.LookRotation(closestEnemy), autoRotationSmoothness);
        }

        private void RotateToJoystickDir(Joystick joystick, float rotationSmoothness)
        {
            Vector3 moveDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

            transform.rotation = Quaternion.Lerp(
                transform.rotation, Quaternion.LookRotation(moveDir), rotationSmoothness);
        }

        private IEnumerator UpdateClosestEnemy()
        {
            while (true)
            {
                ComponentSearcher<ZombieContainer>.InRadius(
                    transform.position, weaponsController.CurrentGun.Distance, out List<ZombieContainer> closestEnemies);

                bool enemyInRange = false;
                ZombieContainer closestEnemy = null;
                if (closestEnemies != null)
                {
                    closestEnemy = ComponentSearcher<ZombieContainer>.Closest(transform.position, closestEnemies);

                    if (closestEnemy && closestEnemy.HealthController.Health > 0)
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.transform.position);
                        enemyInRange = distanceToEnemy <= weaponsController.CurrentGun.Distance;
                    }
                }
                if (enemyInRange)
                    this.closestEnemy = closestEnemy;
                else
                    this.closestEnemy = null;

                yield return new WaitForSeconds(timeToUpdateClosestEnemy);
            }
        }

        public void OnDestroy()
        {
            CoroutineHelper.StopRoutine(ClosestEnemyCoroutine);
        }
    }
}
