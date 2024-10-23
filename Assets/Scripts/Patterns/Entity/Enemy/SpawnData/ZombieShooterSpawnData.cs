using Factory;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyLib
{
    public class ZombieShooterSpawnData : EnemySpawnData
    {
        [SerializeField] private BulletTrailPool _shotPool;

        public override void Initialize(List<Transform> spawnPoses, IEntity target)
        {
            _factory = new ZombieShooterFactory((IPool<ZombieShooterContainer>)_enemyPool.Pool, _ammoPool.Pool, 
                spawnPoses, target, _waveBoostData, _shotPool.Pool);
        }
    }
}
