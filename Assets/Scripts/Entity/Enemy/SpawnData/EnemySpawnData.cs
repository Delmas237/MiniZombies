using Factory;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyLib
{
    public abstract class EnemySpawnData : MonoBehaviour
    {
        [SerializeField] protected EnemyWaveBoostData _waveBoostData;
        public EnemyWaveBoostData EnemyWaveBoostData => _waveBoostData;

        [SerializeField] protected AmmoPackPool _ammoPool;

        [SerializeField] protected EnemyPool _enemyPool;
        public EnemyPool EnemyPool => _enemyPool;
        public abstract IInstanceProvider<IEnemy> Factory { get; }

        public abstract void Initialize(List<Transform> spawnPoses, IEntity target);
    }
}
