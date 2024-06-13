using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieShooterContainer : ZombieContainer
    {
        [Space(10), Header("Controllers")]
        [SerializeField] protected ZombieShooterAttackController _attackController;
        public override IEnemyAttackController AttackController => _attackController;
        
        [SerializeField] protected WeaponsController _weaponsController;
        public IWeaponsController WeaponsController => _weaponsController;

        protected override void Awake()
        {
            base.Awake();

            _healthController.Initialize();
            _animationController.Initialize(HealthController, MoveController, AttackController, GetComponent<Animator>());
            _weaponsController.Initialize();
            _attackController.Initialize(MoveController, _weaponsController, transform);
            _moveController.Initialize(GetComponent<NavMeshAgent>(), transform, AttackController);

            DelayedDisableModule.Initialize(gameObject, this);
            DropAmmoAfterDeathModule.Initialize(HealthController, transform);
        }

        private void OnEnable()
        {
            _moveController.OnEnable();
        }

        protected virtual void Update()
        {
            _moveController.Move();
            _animationController.MoveAnim();
            _moveController.Rotate();

            _attackController.UpdateState();
            _animationController.AttackAnim();
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            _moveController.Agent.enabled = false;
            AttackController.IsAttack = false;
            enabled = false;
        }

        private void Shoot() => _weaponsController.PullTrigger();
    }
}
