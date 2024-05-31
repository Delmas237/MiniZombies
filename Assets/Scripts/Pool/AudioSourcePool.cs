using UnityEngine;

namespace ObjectPool
{
    public class AudioSourcePool : MonoBehaviour
    {
        [SerializeField] private PoolBase<AudioSource> _pool;
        public IPool<AudioSource> Pool => _pool;

        private void Awake()
        {
            if (_pool.Parent == null)
                _pool.Parent = transform;

            _pool.Initialize();
        }
    }
}
