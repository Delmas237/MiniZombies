using Factory;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyLib
{
    public class ZombieSpawnData : EnemySpawnData
    {
        private IFactory<IEnemy> _factory;
        public override IFactory<IEnemy> Factory => _factory;

        public override void Initialize(List<Transform> spawnPoses, IEntity target)
        {
            _factory = new ZombieFactory(_enemyPool.Pool, _ammoPool.Pool, spawnPoses, target);
        }
    }
}
