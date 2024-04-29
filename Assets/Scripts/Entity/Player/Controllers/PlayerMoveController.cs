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

        private PlayerContainer player;
        private Rigidbody rb;

        private EnemyContainer closestEnemy;
        private const float timeToUpdateClosestEnemy = 0.35f;

        public void Initialize(PlayerContainer _player, Transform _transform)
        {
            player = _player;
            transform = _transform;

            rb = player.GetComponent<Rigidbody>();
            player.HealthController.Died += SetControllableFalse;

            player.StartCoroutine(UpdateClosestEnemy());
        }
        private void SetControllableFalse() => controllable = false;

        public void Move()
        {
            if (controllable)
            {
                if (MoveJoystick.Horizontal == 0 || MoveJoystick.Vertical == 0)
                {
                    rb.velocity = new Vector3(
                        MoveJoystick.Horizontal * maxSpeed,
                        rb.velocity.y,
                        MoveJoystick.Vertical * maxSpeed);
                }
                else
                {
                    rb.velocity = new Vector3(
                        MoveJoystick.Horizontal * maxSpeed / 1.5f,
                        rb.velocity.y,
                        MoveJoystick.Vertical * maxSpeed / 1.5f);
                }
            }
        }

        public void Rotation()
        {
            if (controllable)
            {
                if (closestEnemy)
                {
                    RotateToClosestEnemy(closestEnemy.transform.position);
                }
                else
                {
                    if ((player.WeaponsController.AttackJoystick.Horizontal != 0 ||
                        player.WeaponsController.AttackJoystick.Vertical != 0) && AutoRotate)
                    {
                        RotateToJoystickDir(player.WeaponsController.AttackJoystick, rotationSmoothness);
                    }
                    else if (MoveJoystick.Horizontal != 0 || MoveJoystick.Vertical != 0)
                    {
                        RotateToJoystickDir(MoveJoystick, rotationSmoothness);
                    }
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
        public void RotateToAttackJoystickDir()
        {
            if (closestEnemy == null)
                RotateToJoystickDir(player.WeaponsController.AttackJoystick, 1);
        }

        private IEnumerator UpdateClosestEnemy()
        {
            while (true)
            {
                ComponentSearcher<EnemyContainer>.InRadius(
                    transform.position, player.WeaponsController.CurrentGun.Distance, out List<EnemyContainer> closestEnemies);

                bool enemyInRange = false;
                EnemyContainer closestEnemy = null;
                if (closestEnemies != null)
                {
                    closestEnemy = ComponentSearcher<EnemyContainer>.Closest(transform.position, closestEnemies);

                    if (closestEnemy && closestEnemy.HealthController.Health > 0)
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.transform.position);
                        enemyInRange = distanceToEnemy <= player.WeaponsController.CurrentGun.Distance;
                    }
                }
                if (enemyInRange)
                    this.closestEnemy = closestEnemy;
                else
                    this.closestEnemy = null;

                yield return new WaitForSeconds(timeToUpdateClosestEnemy);
            }
        }
    }
}
