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

        public ZombieShooterFactory(IPool<EntityBase> pool, IPool<AmmoPack> ammoPackPool, List<Transform> spawnDots, 
            IEntity target, EnemyWaveBoostData waveBoostData, IInstanceProvider<BulletTrail> bulletPool) : base(pool, ammoPackPool, spawnDots, target, waveBoostData)
        {
            _bulletPool = bulletPool;
        }

        public override void Construct(EntityBase enemy)
        {
            base.Construct(enemy);

            var weaponModule = enemy.GetModule<IEntityWeaponModule>();
            weaponModule.CurrentGun.BulletPool = _bulletPool;
        }
    }
}
