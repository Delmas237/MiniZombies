using UnityEngine;

namespace Entity.Hostile
{
    public interface IEnemyThrowerAttackModule : IEnemyAttackModule
    {
        IInstanceProvider<PoisonProjectile> ProjectileProvider { get; set; }
        IInstanceProvider<ParticleSystem> ProjectileEffectProvider { get; set; }

        void Throw();
    }
}
