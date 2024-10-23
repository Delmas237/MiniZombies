using System;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class ZombieThrowerAttackController : ZombieShooterAttackController, IEnemyThrowerAttackController
    {
        public IInstanceProvider<PoisonProjectile> ProjectileProvider { get; set; }
        public IInstanceProvider<ParticleSystem> ProjectileEffectProvider { get; set; }

        public void Initialize(IEnemyMoveController moveController, Transform transform)
        {
            _moveController = moveController;
            _transform = transform;
            Speed = DefaultSpeed;
        }

        public override void UpdateState()
        {
            bool targetDied = _moveController.Target.HealthController.Health <= 0;
            bool destinationCompleted = Vector3.Distance(_transform.position, _moveController.Agent.destination) < 1f;

            if (!IsAttack && !targetDied)
            {
                if (destinationCompleted)
                    GetIntoPosition();
                else
                    GetOutPosition();
            }
            else if (!destinationCompleted)
            {
                GetOutPosition();
            }
        }

        public void Throw()
        {
            PoisonProjectile poisonProjectile = ProjectileProvider.GetInstance();
            poisonProjectile.Initialize(_transform.position, _moveController.Target.Transform.position);
        }
    }
}
