using UnityEngine;

namespace ObjectPool
{
    public class PoolParticleSystem : MonoBehaviour, IPool<ParticleSystem>
    {
        [SerializeField] private ParticleSystem particlesSystem;
        [SerializeField] private int amount = 20;
        [SerializeField] private bool autoExpand = true;

        private PoolBase<ParticleSystem> pool;

        private void Awake()
        {
            pool = new PoolBase<ParticleSystem>(particlesSystem, amount, transform)
            {
                AutoExpand = autoExpand
            };
        }

        public ParticleSystem GetFreeElement() => pool.GetFreeElement();
    }
}
