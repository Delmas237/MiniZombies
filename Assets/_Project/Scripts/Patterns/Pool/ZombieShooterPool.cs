using Entity.Hostile;
using UnityEngine;

namespace ObjectPool
{
    public class ZombieShooterPool : EnemyPool
    {
        [SerializeField] private PoolBase<ZombieShooterEntity> _pool;
        public override IPool<ZombieEntity> Pool => _pool;

        private void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
