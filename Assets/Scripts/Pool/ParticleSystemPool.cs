using UnityEngine;

namespace ObjectPool
{
    public class ParticleSystemPool : MonoBehaviour
    {
        [SerializeField] private PoolBase<ParticleSystem> _pool;
        public IPool<ParticleSystem> Pool => _pool;

        private void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
