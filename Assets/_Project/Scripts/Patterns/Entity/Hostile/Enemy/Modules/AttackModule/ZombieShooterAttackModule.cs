using System;
using System.Collections;
using UnityEngine;

namespace Entity.Hostile
{
    [Serializable]
    public class ZombieShooterAttackModule : IEnemyAttackModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField, Range(0.01f, 3f)] private float _defaultSpeed = 1f;
        [SerializeField] private int _damage = 15;
        [SerializeField] private float _cooldown = 1f;

        [Space(10), Tooltip("Delay before shooting first time")]
        [SerializeField] protected float _shootDelay = 1f;

        protected Transform _transform;
        protected IEntityTargetModule _targetModule;
        protected IEnemyMovementModule _moveModule;
        protected IEntityWeaponModule _weaponModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public bool IsAttack { get; set; }
        public float Speed { get; set; }

        public float DefaultSpeed => _defaultSpeed;
        public int Damage => _damage;

        public void Initialize(Transform transform, IEntityTargetModule targetModule, IEnemyMovementModule moveModule, IEntityWeaponModule weaponModule)
        {
            _transform = transform;
            _targetModule = targetModule;
            _moveModule = moveModule;
            _weaponModule = weaponModule;

            Speed = DefaultSpeed;
            _weaponModule.CurrentGun.Damage = _damage;
            _weaponModule.CurrentGun.Cooldown = _cooldown;
        }

        public virtual void UpdateData()
        {
            Speed /= _cooldown;
        }

        public virtual void UpdateState()
        {
            float distanceToTarget = Vector3.Distance(_targetModule.Target.Transform.position, _transform.position);
            bool targetDied = _targetModule.Target.HealthModule.Health <= 0;

            if (!IsAttack && !targetDied)
            {
                if (distanceToTarget < _weaponModule.CurrentGun.Distance / 2)
                {
                    GetIntoPosition();
                }
            }
            else
            {
                if (distanceToTarget > _weaponModule.CurrentGun.Distance || targetDied)
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
