using System;
using System.Collections;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class ZombieShooterAttackController : IEnemyAttackController
    {
        [SerializeField, Range(0.01f, 3f)] protected float _defaultSpeed = 1f;
        [SerializeField] private int _damage = 15;
        [SerializeField] private float _cooldown = 1f;

        [Space(10), Tooltip("Delay before shooting first time")]
        [SerializeField] protected float _shootDelay = 1f;

        protected IEnemyMoveController _moveController;
        protected IWeaponsController _weaponsController;
        protected Transform _transform;

        public bool IsAttack { get; set; }
        public float Speed { get; set; }

        public float DefaultSpeed => _defaultSpeed;
        public int Damage => _damage;

        public void Initialize(IEnemyMoveController moveController, IWeaponsController weaponsController, Transform transform)
        {
            _moveController = moveController;
            _weaponsController = weaponsController;
            _transform = transform;

            Speed = DefaultSpeed;
            _weaponsController.CurrentGun.Damage = _damage;
            _weaponsController.CurrentGun.Cooldown = _cooldown;
        }

        public virtual void UpdateData()
        {
            Speed /= _cooldown;
        }

        public virtual void UpdateState()
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

        protected virtual void GetIntoPosition()
        {
            IsAttack = true;
            CoroutineHelper.StartRoutine(ShootDelay(_shootDelay));

            if (_moveController.Agent.enabled)
                _moveController.Agent.isStopped = true;
        }
        protected IEnumerator ShootDelay(float delay)
        {
            float speedX = Speed;

            Speed = 0;
            yield return new WaitForSeconds(delay);
            Speed = speedX;
        }

        protected virtual void GetOutPosition()
        {
            IsAttack = false;

            if (_moveController.Agent.enabled)
                _moveController.Agent.isStopped = false;
        }
    }
}
