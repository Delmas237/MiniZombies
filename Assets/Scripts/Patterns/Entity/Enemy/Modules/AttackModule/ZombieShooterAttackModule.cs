using System;
using System.Collections;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class ZombieShooterAttackModule : IEnemyAttackModule
    {
        [SerializeField, Range(0.01f, 3f)] protected float _defaultSpeed = 1f;
        [SerializeField] private int _damage = 15;
        [SerializeField] private float _cooldown = 1f;

        [Space(10), Tooltip("Delay before shooting first time")]
        [SerializeField] protected float _shootDelay = 1f;

        protected IEnemyMoveModule _moveModule;
        protected IWeaponsModule _weaponsModule;
        protected Transform _transform;

        public bool IsAttack { get; set; }
        public float Speed { get; set; }

        public float DefaultSpeed => _defaultSpeed;
        public int Damage => _damage;

        public void Initialize(IEnemyMoveModule moveModule, IWeaponsModule weaponsModule, Transform transform)
        {
            _moveModule = moveModule;
            _weaponsModule = weaponsModule;
            _transform = transform;

            Speed = DefaultSpeed;
            _weaponsModule.CurrentGun.Damage = _damage;
            _weaponsModule.CurrentGun.Cooldown = _cooldown;
        }

        public virtual void UpdateData()
        {
            Speed /= _cooldown;
        }

        public virtual void UpdateState()
        {
            float distanceToTarget = Vector3.Distance(_moveModule.Target.Transform.position, _transform.position);
            bool targetDied = _moveModule.Target.HealthModule.Health <= 0;

            if (!IsAttack && !targetDied)
            {
                if (distanceToTarget < _weaponsModule.CurrentGun.Distance / 2)
                {
                    GetIntoPosition();
                }
            }
            else
            {
                if (distanceToTarget > _weaponsModule.CurrentGun.Distance || targetDied)
                {
                    GetOutPosition();
                }
            }
        }

        protected virtual void GetIntoPosition()
        {
            IsAttack = true;
            CoroutineHelper.StartRoutine(ShootDelay(_shootDelay));

            if (_moveModule.Agent.enabled)
                _moveModule.Agent.isStopped = true;
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

            if (_moveModule.Agent.enabled)
                _moveModule.Agent.isStopped = false;
        }
    }
}
