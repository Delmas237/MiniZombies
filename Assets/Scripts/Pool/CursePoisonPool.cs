using EnemyLib;
using UnityEngine;

namespace ObjectPool
{
    public class CursePoisonPool : MonoBehaviour
    {
        [SerializeField] private PoolBase<PoisonProjectile> _pool;
        public IPool<PoisonProjectile> Pool => _pool;

        private void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
