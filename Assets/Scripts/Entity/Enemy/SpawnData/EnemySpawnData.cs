using ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyLib
{
    public abstract class EnemySpawnData : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] protected float _priority = 100;
        public float Priority => _priority;
        [Space(10)]
        [SerializeField] protected EnemyWaveBoostData _waveBoostData;
        public EnemyWaveBoostData EnemyWaveBoostData => _waveBoostData;

        [SerializeField] protected AmmoPackPool _ammoPool;

        [SerializeField] protected EnemyPool _enemyPool;
        public EnemyPool EnemyPool => _enemyPool;

        protected IInstanceProvider<IEnemy> _factory;
        public IInstanceProvider<IEnemy> Factory => _factory;

        public abstract void Initialize(List<Transform> spawnPoses, IEntity target);
    }
}
