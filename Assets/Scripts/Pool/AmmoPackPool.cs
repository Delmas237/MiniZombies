using EnemyLib;
using UnityEngine;
using Weapons;

namespace ObjectPool
{
    public class AmmoPackPool : MonoBehaviour
    {
        [SerializeField] private PoolBase<AmmoPack> _pool;
        public IPool<AmmoPack> Pool => _pool;

        private void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
