using UnityEngine;

namespace ObjectPool
{
    public class PoolParticleSystem : MonoBehaviour, IPool<ParticleSystem>
    {
        [SerializeField] private ParticleSystem _particlesSystem;
        [SerializeField] private int _amount = 20;
        [SerializeField] private bool _autoExpand = true;

        private PoolBase<ParticleSystem> _pool;

        private void Awake()
        {
            _pool = new PoolBase<ParticleSystem>(_particlesSystem, _amount, transform)
            {
                AutoExpand = _autoExpand
            };
        }

        public ParticleSystem GetFreeElement() => _pool.GetFreeElement();
    }
}
