using EntityLib;
using EntityLib.Hostile;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Factory
{
    public class EnemyThrowerFactory : EnemyFactory
    {
        protected IPool<PoisonProjectile> _projectilePool;
        protected IPool<ParticleSystem> _projectileEffectPool;

        public EnemyThrowerFactory(IPool<Entity> pool, IPool<AmmoPack> ammoPackPool, List<Transform> spawnDots,
            IEntity target, EnemyWaveBoostData waveBoostData, IPool<PoisonProjectile> projectilePool, IPool<ParticleSystem> projectileEffectPool) : base(pool, ammoPackPool, spawnDots, target, waveBoostData)
        {
            _projectilePool = projectilePool;
            _projectileEffectPool = projectileEffectPool;
        }

        public override void Construct(Entity enemy)
        {
            base.Construct(enemy);
            var attackModule = enemy.GetModule<IEnemyThrowerAttackModule>();

            attackModule.ProjectileProvider = _projectilePool;
            attackModule.ProjectileEffectProvider = _projectileEffectPool;
        }
    }
}
