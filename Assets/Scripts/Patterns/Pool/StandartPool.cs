using UnityEngine;

namespace ObjectPool
{
    public class StandartPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] protected PoolBase<T> _pool;
        public IPool<T> Pool => _pool;

        protected virtual void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
