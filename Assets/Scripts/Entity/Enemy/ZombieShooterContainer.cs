using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    public class ZombieShooterContainer : ZombieContainer
    {
        [field: Space(10)]
        [field: Header("Controllers")]
        [field: SerializeField] public WeaponsController WeaponsController { get; set; }
        [field: SerializeField] public ZombieShooterAttackController AttackController { get; set; }

        protected override void Start()
        {
            base.Start();

            _healthController.Initialize();
            _animationController.Initialize(HealthController, MoveController, AttackController, GetComponent<Animator>());
            WeaponsController.Initialize(HealthController);
            AttackController.Initialize(MoveController, WeaponsController, AnimationController, transform);
            _moveController.Initialize(GetComponent<NavMeshAgent>());

            DropAmmoAfterDeathModule.Initialize(HealthController, transform);
        }

        protected virtual void Update()
        {
            _moveController.Move();
            _animationController.MoveAnim();

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
    }
}
