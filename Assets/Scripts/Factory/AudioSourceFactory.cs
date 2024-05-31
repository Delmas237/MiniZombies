using ObjectPool;
using UnityEngine;

namespace Factory
{
    public class AudioSourceFactory : FactoryBase<AudioSource>, IFactory<AudioSource>
    {
        private readonly PoolBase<AudioSource> _pool;
        public PoolBase<AudioSource> Pool => _pool;

        public AudioSourceFactory(PoolBase<AudioSource> pool, AudioSource prefab) : base(prefab) 
        {
            _pool = pool;
        }

        public AudioSource GetInstance()
        {
            AudioSource instance = _pool.GetFreeElement();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        protected override void ReconstructToDefault(AudioSource audioSource) { }

        protected override void Construct(AudioSource audioSource)
        {
            audioSource.Play();
        }
    }
}
