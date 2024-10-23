using EnemyLib;
using UnityEngine;

namespace ObjectPool
{
    public class ZombiePool : EnemyPool
    {
        [SerializeField] private PoolBase<ZombieContainer> _pool;
        public override IPool<ZombieContainer> Pool => _pool;

        private void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
