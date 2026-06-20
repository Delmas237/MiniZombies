using Entity.Hostile;
using UnityEngine;

namespace ObjectPool
{
    public class ZombieThrowerPool : EnemyPool
    {
        [SerializeField] private PoolBase<ZombieThrowerEntity> _pool;
        public override IPool<ZombieEntity> Pool => _pool;

        private void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
