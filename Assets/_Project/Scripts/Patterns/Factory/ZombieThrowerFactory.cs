using EnemyLib;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Factory
{
    public class ZombieThrowerFactory : ZombieFactory
    {
        protected IPool<PoisonProjectile> _projectilePool;
        protected IPool<ParticleSystem> _projectileEffectPool;

        public ZombieThrowerFactory(IPool<ZombieThrowerEntity> pool, IPool<AmmoPack> ammoPackPool, List<Transform> spawnDots,
            IEntity target, EnemyWaveBoostData waveBoostData, IPool<PoisonProjectile> projectilePool, IPool<ParticleSystem> projectileEffectPool) : base(pool, ammoPackPool, spawnDots, target, waveBoostData)
        {
            _projectilePool = projectilePool;
            _projectileEffectPool = projectileEffectPool;
        }

        public override IHostile GetInstance()
        {
            var pool = (IPool<ZombieThrowerEntity>)_pool;
            ZombieThrowerEntity instance = pool.GetInstance();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        protected void Construct(ZombieThrowerEntity enemy)
        {
            base.Construct(enemy);
            enemy.ThrowerAttackModule.ProjectileProvider = _projectilePool;
            enemy.ThrowerAttackModule.ProjectileEffectProvider = _projectileEffectPool;
        }
    }
}
