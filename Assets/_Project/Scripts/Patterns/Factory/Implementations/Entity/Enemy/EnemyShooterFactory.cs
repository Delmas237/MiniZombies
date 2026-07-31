using EntityLib;
using EntityLib.Hostile;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Factory
{
    public class EnemyShooterFactory : EnemyFactory
    {
        protected IInstanceProvider<BulletTrail> _bulletPool;

        public EnemyShooterFactory(IPool<Entity> pool, IPool<AmmoPack> ammoPackPool, List<Transform> spawnDots, 
            IEntity target, EnemyWaveBoostData waveBoostData, IInstanceProvider<BulletTrail> bulletPool) : base(pool, ammoPackPool, spawnDots, target, waveBoostData)
        {
            _bulletPool = bulletPool;
        }

        public override void Construct(Entity enemy)
        {
            base.Construct(enemy);

            var weaponModule = enemy.GetModule<IEntityWeaponModule>();
            weaponModule.CurrentGun.BulletPool = _bulletPool;
        }
    }
}
