using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieShooterEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyMovementModule _moveModule;
        [SerializeField] protected ZombieShooterAttackModule _attackModule;
        [SerializeField] protected WeaponModule _weaponsModule;

        public override IEnemyMovementModule MovementModule => _moveModule;
        public override IEnemyAttackModule AttackModule => _attackModule;
        public IWeaponModule WeaponModule => _weaponsModule;

        protected override void Awake()
        {
            base.Awake();

            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(HealthModule, MovementModule, AttackModule, GetComponent<Animator>());
            _weaponsModule.Initialize();
            _attackModule.Initialize(MovementModule, WeaponModule, transform);
            _moveModule.Initialize(GetComponent<NavMeshAgent>(), transform, AttackModule);

            _delayedDisableModule.Initialize(gameObject, this);
            _dropAmmoAfterDeathModule.Initialize(HealthModule, transform);
        }

        protected override void OnEnable()
        {
            _attackModule.UpdateData();
            _moveModule.UpdateData();
            base.OnEnable();
        }

        protected virtual void Update()
        {
            _moveModule.Move();
            _animationModule.MoveAnim();
            _moveModule.Rotate();

            _attackModule.UpdateState();
            _animationModule.AttackAnim();
        }

        protected override void OnHealhIsOver()
        {
            base.OnHealhIsOver();

            _moveModule.Agent.enabled = false;
            _attackModule.IsAttack = false;
            enabled = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _animationModule.OnDestroy();
        }

        private void Shoot() => _weaponsModule.PullTrigger();
    }
}
