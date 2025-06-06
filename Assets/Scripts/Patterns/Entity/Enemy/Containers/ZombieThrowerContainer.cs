using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieThrowerContainer : ZombieContainer
    {
        [Header("Modules")]
        [SerializeField] protected EnemyAvoidantMoveModule _moveModule;
        [SerializeField] protected ZombieThrowerAttackModule _attackModule;

        public IEnemyThrowerAttackModule ThrowerAttackModule => _attackModule;
        public override IEnemyMoveModule MoveModule => _moveModule;
        public override IEnemyAttackModule AttackModule => _attackModule;

        protected override void Awake()
        {
            base.Awake();

            _healthModule.Initialize();
            _audioModule.Initialize(HealthModule);
            _animationModule.Initialize(HealthModule, MoveModule, AttackModule, GetComponent<Animator>());
            _attackModule.Initialize(MoveModule, transform);
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

        private void Shoot() => ThrowerAttackModule.Throw();
    }
}
