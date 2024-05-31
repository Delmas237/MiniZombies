using EnemyLib;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Factory
{
    public class ZombieShooterFactory : ZombieFactory
    {
        protected IPool<ParticleSystem> _shotPool;

        public ZombieShooterFactory(IPool<ZombieShooterContainer> pool, IPool<AmmoPack> ammoPackPool, List<Transform> spawnDots, 
            IEntity target, IPool<ParticleSystem> shotPool) : base(pool, ammoPackPool, spawnDots, target)
        {
            _shotPool = shotPool;
        }

        public override IEnemy GetInstance()
        {
            var pool = (IPool<ZombieShooterContainer>)_pool;
            ZombieShooterContainer instance = pool.GetFreeElement();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        protected void Construct(ZombieShooterContainer enemy)
        {
            base.Construct(enemy);

            enemy.WeaponsController.ChangeGun(GunType.Pistol);
            enemy.WeaponsController.CurrentGun.ShotPool = _shotPool;
        }
    }
}
