using EnemyLib;
using Factory;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyLib
{
    public class ZombieThrowerSpawnData : ZombieSpawnData
    {
        [SerializeField] private CursePoisonPool _projectilePool;
        [SerializeField] private ParticleSystemPool _projectileEffectPool;

        public override void Initialize(List<Transform> spawnPoses, IEntity target)
        {
            _factory = new ZombieThrowerFactory((IPool<ZombieThrowerContainer>)_enemyPool.Pool, 
                _ammoPool.Pool, spawnPoses, target, _waveBoostData, _projectilePool.Pool, _projectileEffectPool.Pool);
        }
    }
}
