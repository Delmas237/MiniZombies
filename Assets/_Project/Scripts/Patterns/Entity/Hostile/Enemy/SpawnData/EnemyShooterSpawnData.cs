using Factory;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace EntityLib.Hostile
{
    public class EnemyShooterSpawnData : EnemySpawnData
    {
        [SerializeField] private BulletTrailPool _shotPool;

        public override void Initialize(List<Transform> spawnPoses, IEntity target)
        {
            _factory = new EnemyShooterFactory(
                _enemyPool.Pool, _ammoPool.Pool, 
                spawnPoses, target, _waveBoostData, 
                _shotPool.Pool);
        }
    }
}
