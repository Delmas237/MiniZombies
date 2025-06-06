using UnityEngine;
using UnityEngine.AI;
using Weapons;

namespace EnemyLib
{
    public class ZombieThrowerContainer : ZombieContainer
    {
        [Space(10), Header("Controllers")]
        [SerializeField] protected EnemyAvoidantMoveController _moveController;
        [SerializeField] protected ZombieThrowerAttackController _attackController;

        public IEnemyThrowerAttackController ThrowerAttackController => _attackController;
        public override IEnemyMoveController MoveController => _moveController;
        public override IEnemyAttackController AttackController => _attackController;

        protected override void Awake()
        {
            base.Awake();

            _healthController.Initialize();
            _audioController.Initialize(_healthController);
            _animationController.Initialize(HealthController, MoveController, AttackController, GetComponent<Animator>());
            _attackController.Initialize(MoveController, transform);
            _moveController.Initialize(GetComponent<NavMeshAgent>(), transform, AttackController);

            DelayedDisableModule.Initialize(gameObject, this);
            DropAmmoAfterDeathModule.Initialize(HealthController, transform);
        }

        protected override void OnEnable()
        {
            _attackController.UpdateData();
            _moveController.UpdateData();
            base.OnEnable();
        }

        protected virtual void Update()
        {
            _moveController.Move();
            _animationController.MoveAnim();
            _moveController.Rotate();

            _attackController.UpdateState();
            _animationController.AttackAnim();
        }

        protected override void OnHealhIsOver()
        {
            base.OnHealhIsOver();

            _moveController.Agent.enabled = false;
            AttackController.IsAttack = false;
            enabled = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _animationController.OnDestroy();
        }

        private void Shoot() => ThrowerAttackController.Throw();
    }
}
