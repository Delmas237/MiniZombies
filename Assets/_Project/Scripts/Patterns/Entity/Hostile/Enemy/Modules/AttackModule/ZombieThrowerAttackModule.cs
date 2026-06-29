using System;
using UnityEngine;

namespace Entity.Hostile
{
    [Serializable]
    public class ZombieThrowerAttackModule : ZombieShooterAttackModule, IEnemyThrowerAttackModule
    {
        public IInstanceProvider<PoisonProjectile> ProjectileProvider { get; set; }
        public IInstanceProvider<ParticleSystem> ProjectileEffectProvider { get; set; }

        public void Initialize(Transform transform, IEntityTargetModule targetModule, IEnemyMovementModule moveModule)
        {
            _transform = transform;
            _targetModule = targetModule;
            _moveModule = moveModule;

            Speed = DefaultSpeed;
        }

        public override void UpdateState()
        {
            if (!_enabled)
                return;

            bool targetDied = _targetModule.Target.HealthModule.Health <= 0;
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
            if (!_enabled)
                return;

            PoisonProjectile poisonProjectile = ProjectileProvider.GetInstance();
            poisonProjectile.Initialize(_transform.position, _targetModule.Target.Transform.position);
        }
    }
}
