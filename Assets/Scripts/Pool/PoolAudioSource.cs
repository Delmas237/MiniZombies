using Factory;
using UnityEngine;

namespace ObjectPool
{
    public class PoolAudioSource : MonoBehaviour,  IPool<AudioSource>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private int amount = 20;
        [SerializeField] private bool autoExpand = true;

        private PoolBase<AudioSource> pool;
        private FactoryAudioSource factory;

        private void Awake()
        {
            factory = new FactoryAudioSource(audioSource, transform);
            pool = new PoolBase<AudioSource>(audioSource, amount, factory)
            {
                AutoExpand = autoExpand
            };
        }

        public AudioSource GetFreeElement()
        {
            AudioSource audioSource = pool.GetFreeElement();
            factory.Construct(audioSource);
            return audioSource;
        }
    }
}
