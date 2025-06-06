using System;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class ZombieThrowerAttackModule : ZombieShooterAttackModule, IEnemyThrowerAttackModule
    {
        public IInstanceProvider<PoisonProjectile> ProjectileProvider { get; set; }
        public IInstanceProvider<ParticleSystem> ProjectileEffectProvider { get; set; }

        public void Initialize(IEnemyMoveModule moveModule, Transform transform)
        {
            _moveModule = moveModule;
            _transform = transform;
            Speed = DefaultSpeed;
        }

        public override void UpdateState()
        {
            bool targetDied = _moveModule.Target.HealthModule.Health <= 0;
            bool destinationCompleted = Vector3.Distance(_transform.position, _moveModule.Agent.destination) < 1f;

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
            poisonProjectile.Initialize(_transform.position, _moveModule.Target.Transform.position);
        }
    }
}
