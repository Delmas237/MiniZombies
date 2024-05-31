using System;
using System.Collections;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class ZombieShooterAttackController : IEnemyAttackController
    {
        public bool IsAttack { get; set; }
        public float AttackSpeed { get; set; } = 1;

        [SerializeField] private int _damage = 15;
        public int Damage => _damage;

        [SerializeField] private float _cooldown = 1f;
        [Space(10)]
        [SerializeField, Tooltip("Delay before firing")] private float _shootDelay = 1f;

        private IEnemyMoveController _moveController;
        private IWeaponsController _weaponsController;
        private Transform _transform;

        public void Initialize(IEnemyMoveController moveController, IWeaponsController weaponsController, Transform transform)
        {
            _moveController = moveController;
            _weaponsController = weaponsController;
            _transform = transform;

            _weaponsController.CurrentGun.Damage = _damage;
            _weaponsController.CurrentGun.Cooldown = _cooldown;
            AttackSpeed /= _cooldown;
        }

        public void UpdateState()
        {
            float distanceToTarget = Vector3.Distance(_moveController.Target.Transform.position, _transform.position);
            bool targetDied = _moveController.Target.HealthController.Health <= 0;

            if (!IsAttack && !targetDied)
            {
                if (distanceToTarget < _weaponsController.CurrentGun.Distance / 2)
                {
                    GetIntoPosition();
                }
            }
            else
            {
                if (distanceToTarget > _weaponsController.CurrentGun.Distance || targetDied)
                {
                    GetOutPosition();
                }
            }
        }

        private void GetIntoPosition()
        {
            IsAttack = true;
            CoroutineHelper.StartRoutine(ShootDelay(_shootDelay));

            if (_moveController.Agent.enabled)
                _moveController.Agent.isStopped = true;
        }

        private IEnumerator ShootDelay(float delay)
        {
            float speedX = AttackSpeed;

            AttackSpeed = 0;
            yield return new WaitForSeconds(delay);
            AttackSpeed = speedX;
        }

        private void GetOutPosition()
        {
            IsAttack = false;

            if (_moveController.Agent.enabled)
                _moveController.Agent.isStopped = false;
        }
    }
}
