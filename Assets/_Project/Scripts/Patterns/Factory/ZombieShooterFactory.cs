using Entity;
using Entity.Hostile;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Factory
{
    public class ZombieShooterFactory : ZombieFactory
    {
        protected IInstanceProvider<BulletTrail> _bulletPool;

        public ZombieShooterFactory(IPool<ZombieShooterEntity> pool, IPool<AmmoPack> ammoPackPool, List<Transform> spawnDots, 
            IEntity target, EnemyWaveBoostData waveBoostData, IInstanceProvider<BulletTrail> bulletPool) : base(pool, ammoPackPool, spawnDots, target, waveBoostData)
        {
            _bulletPool = bulletPool;
        }

        public override IHostile GetInstance()
        {
            var pool = (IInstanceProvider<ZombieShooterEntity>)_pool;
            ZombieShooterEntity instance = pool.GetInstance();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        protected void Construct(ZombieShooterEntity enemy)
        {
            base.Construct(enemy);
            enemy.WeaponModule.CurrentGun.BulletPool = _bulletPool;
        }
    }
}
