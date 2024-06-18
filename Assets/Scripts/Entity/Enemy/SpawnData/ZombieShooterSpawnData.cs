using Factory;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyLib
{
    public class ZombieShooterSpawnData : EnemySpawnData
    {
        [SerializeField] private ParticleSystemPool _shotPool;
        public ParticleSystemPool ShotPool => _shotPool;

        private IInstanceProvider<IEnemy> _factory;
        public override IInstanceProvider<IEnemy> Factory => _factory;

        public override void Initialize(List<Transform> spawnPoses, IEntity target)
        {
            _factory = new ZombieShooterFactory((IPool<ZombieShooterContainer>)_enemyPool.Pool, _ammoPool.Pool, spawnPoses, target, _waveBoostData, _shotPool.Pool);
        }
    }
}
