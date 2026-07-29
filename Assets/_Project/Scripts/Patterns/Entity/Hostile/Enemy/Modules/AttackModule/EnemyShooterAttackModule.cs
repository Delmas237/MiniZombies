using System;
using System.Collections;
using UnityEngine;

namespace Entity.Hostile
{
    [Serializable]
    public class EnemyShooterAttackModule : IEnemyAttackModule, IOnStart, IUpdatable, IDisposable
    {
        [SerializeField] protected bool _enabled = true;
        [Space(10)]
        [SerializeField] protected int _damage = 15;
        [SerializeField, Range(0.01f, 3f)] protected float _baseSpeed = 1f;
        [Space(10)]
        [SerializeField] protected float _attackDelay = 1f;
        [SerializeField] protected float _cooldown = 1f;

        protected bool _isAttack;

        protected Coroutine _attackCoroutine;

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
        public float BaseSpeed => _baseSpeed;
        public int Damage => _damage;

        [ModuleInject]
        private void Initialize(Transform transform, IEntityTargetModule targetModule, IEnemyMovementModule moveModule, IEntityWeaponModule weaponModule)
        {
            _transform = transform;
            _targetModule = targetModule;
            _moveModule = moveModule;
            _weaponModule = weaponModule;
        }

        public virtual void Start()
        {
            _weaponModule.CurrentGun.Damage = _damage;
            _weaponModule.CurrentGun.Cooldown = _cooldown;
        }

        public virtual void Update()
        {
            UpdateState();
        }

        protected virtual void UpdateState()
        {
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
                if (distanceToTarget < attackDistance)
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
            if (_moveModule.Agent != null && _moveModule.Agent.enabled)
                _moveModule.Agent.isStopped = true;

            Attack();
        }
        protected virtual void GetOutPosition()
        {
            StopAttackImmediately();
        }

        public virtual void Attack()
        {
            if (!Enabled)
                return;

            _isAttack = true;
            _attackCoroutine = CoroutineHelper.StartRoutine(AttackCoroutine());
        }
        protected IEnumerator AttackCoroutine()
        {
            while (_isAttack)
            {
                yield return new WaitForSeconds(_attackDelay / Speed);
                
                if (!_isAttack)
                    yield break;

                Perform();

                yield return new WaitForSeconds(_cooldown);
            }
        }
        protected virtual void Perform()
        {
            _weaponModule.PullTrigger();
        }

        public void StopAttackImmediately()
        {
            _isAttack = false;

            if (_moveModule.Agent != null && _moveModule.Agent.enabled && _moveModule.Agent.isOnNavMesh)
                _moveModule.Agent.isStopped = false;

            if (_attackCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }

        public void Dispose()
        {
            StopAttackImmediately();
        }
    }
}