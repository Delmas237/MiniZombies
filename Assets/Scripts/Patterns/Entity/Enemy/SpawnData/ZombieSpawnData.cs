using Factory;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyLib
{
    public class ZombieSpawnData : EnemySpawnData
    {
        public override void Initialize(List<Transform> spawnPoses, IEntity target)
        {
            _factory = new ZombieFactory(_enemyPool.Pool, _ammoPool.Pool, spawnPoses, target, _waveBoostData);
        }
    }
}
