using System;
using System.Collections;
using UnityEngine;

namespace Entity.Hostile
{
    [Serializable]
    public class ZombieShooterAttackModule : IEnemyAttackModule, IDisposable
    {
        [SerializeField] protected bool _enabled = true;
        [Space(10)]
        [SerializeField, Range(0.01f, 3f)] protected float _defaultSpeed = 1f;
        [SerializeField] protected int _damage = 15;
        [SerializeField] protected float _cooldown = 1f;

        [Space(10), Tooltip("Delay before shooting first time")]
        [SerializeField] protected float _shootDelay = 1f;

        protected bool _isAttack;
        protected Coroutine _shootDelayCoroutine;
        
        protected Transform _transform;
        protected IEntityTargetModule _targetModule;
        protected IEnemyMovementModule _moveModule;
        protected IEntityWeaponModule _weaponModule;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (!_enabled)
                        StopAttackImmediately();
                }
            }
        }

        public float Speed { get; set; }

        public bool IsAttack => _isAttack;
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
            if (!_enabled)
                return;

            if (_targetModule.Target == null)
                return;

            if (_targetModule.Target.HealthModule.Health <= 0)
            {
                GetOutPosition();
                return;
            }

            float distanceToTarget = Vector3.Distance(_targetModule.Target.Transform.position, _transform.position);
            float attackDistance = _weaponModule.CurrentGun.Distance;

            if (!_isAttack)
            {
                if (distanceToTarget < attackDistance / 2)
                    GetIntoPosition();
            }
            else
            {
                if (distanceToTarget > attackDistance || _targetModule.Target.HealthModule.Health <= 0)
                    GetOutPosition();
            }
        }

        protected virtual void GetIntoPosition()
        {
            if (!_enabled)
                return;

            _isAttack = true;

            if (_shootDelayCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_shootDelayCoroutine);
                _shootDelayCoroutine = null;
            }

            _shootDelayCoroutine = CoroutineHelper.StartRoutine(ShootDelayCoroutine(_shootDelay));

            if (_moveModule.Agent != null && _moveModule.Agent.enabled)
                _moveModule.Agent.isStopped = true;
        }

        private IEnumerator ShootDelayCoroutine(float delay)
        {
            float speedX = Speed;
            Speed = 0;

            yield return new WaitForSeconds(delay);

            Speed = speedX;
            _shootDelayCoroutine = null;
        }

        protected virtual void GetOutPosition()
        {
            if (!_enabled)
                return;

            StopAttackImmediately();
        }

        public void StopAttackImmediately()
        {
            _isAttack = false;

            if (_moveModule.Agent != null && _moveModule.Agent.enabled && _moveModule.Agent.isOnNavMesh)
                _moveModule.Agent.isStopped = false;

            if (_shootDelayCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_shootDelayCoroutine);
                _shootDelayCoroutine = null;
            }
        }

        public virtual void DealDamage()
        {
            if (!_enabled)
                return;

            if (_targetModule.Target == null)
                return;

            if (_targetModule.Target.HealthModule.Health <= 0)
            {
                GetOutPosition();
                return;
            }

            _targetModule.Target.HealthModule.Decrease(Damage);
        }

        public void Dispose()
        {
            StopAttackImmediately();
        }
    }
}