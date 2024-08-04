using UnityEngine;

namespace EnemyLib
{
    public interface IEnemyThrowerAttackController : IEnemyAttackController
    {
        public IInstanceProvider<PoisonProjectile> ProjectileProvider { get; set; }
        public IInstanceProvider<ParticleSystem> ProjectileEffectProvider { get; set; }

        public void Throw();
    }
}
