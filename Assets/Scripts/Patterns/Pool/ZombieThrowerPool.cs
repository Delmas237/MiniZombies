using EnemyLib;
using UnityEngine;

namespace ObjectPool
{
    public class ZombieThrowerPool : EnemyPool
    {
        [SerializeField] private PoolBase<ZombieThrowerContainer> _pool;
        public override IPool<ZombieContainer> Pool => _pool;

        private void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
