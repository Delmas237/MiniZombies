using UnityEngine;

namespace EnemyLib
{
    public interface IEnemyThrowerAttackModule : IEnemyAttackModule
    {
        public IInstanceProvider<PoisonProjectile> ProjectileProvider { get; set; }
        public IInstanceProvider<ParticleSystem> ProjectileEffectProvider { get; set; }

        public void Throw();
    }
}
