using Factory;
using ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Hostile
{
    public class EnemySpawnData : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] protected float _priority = 100;
        [Space(10)]
        [SerializeField] protected EnemyWaveBoostData _waveBoostData;
        [SerializeField] protected AmmoPackPool _ammoPool;
        [SerializeField] protected EntityPool _enemyPool;

        protected IInstanceProvider<IEntity> _factory;

        public float Priority => _priority;
        public EnemyWaveBoostData EnemyWaveBoostData => _waveBoostData;
        public EntityPool EnemyPool => _enemyPool;
        public IInstanceProvider<IEntity> Factory => _factory;

        public virtual void Initialize(List<Transform> spawnPoses, IEntity target)
        {
            _factory = new EnemyFactory(
                _enemyPool.Pool, _ammoPool.Pool,
                spawnPoses, target, _waveBoostData);
        }
    }
}
