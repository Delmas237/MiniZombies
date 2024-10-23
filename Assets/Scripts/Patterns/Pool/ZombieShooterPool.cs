using EnemyLib;
using UnityEngine;

namespace ObjectPool
{
    public class ZombieShooterPool : EnemyPool
    {
        [SerializeField] private PoolBase<ZombieShooterContainer> _shooterPool;
        public override IPool<ZombieContainer> Pool => _shooterPool;

        private void Awake()
        {
            if (_shooterPool.Parent == null)
                _shooterPool.Parent = transform;

            _shooterPool.Initialize();
        }
    }
}
