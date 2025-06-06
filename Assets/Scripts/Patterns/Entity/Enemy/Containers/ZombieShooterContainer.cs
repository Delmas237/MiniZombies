using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieShooterContainer : ZombieContainer
    {
        [Header("Modules")]
        [SerializeField] protected EnemyMoveModule _moveModule;
        [SerializeField] protected ZombieShooterAttackModule _attackModule;
        [SerializeField] protected WeaponsModule _weaponsModule;

        public override IEnemyMoveModule MoveModule => _moveModule;
        public override IEnemyAttackModule AttackModule => _attackModule;
        public IWeaponsModule WeaponsModule => _weaponsModule;

        protected override void Awake()
        {
            base.Awake();

            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(HealthModule, MoveModule, AttackModule, GetComponent<Animator>());
            _weaponsModule.Initialize();
            _attackModule.Initialize(MoveModule, WeaponsModule, transform);
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
