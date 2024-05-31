using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieShooterContainer : ZombieContainer
    {
        [field: Space(10), Header("Controllers")]
        [field: SerializeField] public WeaponsController WeaponsController { get; set; }
        [field: SerializeField] public ZombieShooterAttackController AttackController { get; set; }

        protected override void Start()
        {
            base.Start();

            _healthController.Initialize();
            _animationController.Initialize(HealthController, MoveController, AttackController, GetComponent<Animator>());
            WeaponsController.Initialize(HealthController);
            AttackController.Initialize(MoveController, WeaponsController, AnimationController, transform);
            _moveController.Initialize(GetComponent<NavMeshAgent>(), transform, AttackController);

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
            _moveController.Rotation();

            AttackController.UpdateState();
            _animationController.AttackAnim();
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            _moveController.Agent.enabled = false;
            AttackController.IsAttack = false;
            enabled = false;
        }

        private void Shoot() => WeaponsController.PullTrigger();
    }
}
