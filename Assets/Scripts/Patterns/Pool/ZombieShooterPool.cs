using EnemyLib;
using UnityEngine;

namespace ObjectPool
{
    public class ZombieShooterPool : EnemyPool
    {
        [SerializeField] private PoolBase<ZombieShooterContainer> _pool;
        public override IPool<ZombieContainer> Pool => _pool;

        private void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
