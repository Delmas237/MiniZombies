using ObjectPool;
using UnityEngine;

namespace Factory
{
    public class AudioSourceFactory : MonoBehaviour, IFactory<AudioSource>, IInstanceProvider<AudioSource>
    {
        [SerializeField] private AudioSourcePool _audioSourcePool;
        private IPool<AudioSource> _pool;
        private AudioSource[] _prefab;

        public IPool<AudioSource> Pool => _pool;
        public AudioSource[] Prefabs => _prefab;

        private void Awake()
        {
            _pool = _audioSourcePool.Pool;
            _prefab = _audioSourcePool.Pool.Prefabs;
        }

        public AudioSource GetInstance()
        {
            AudioSource instance = _audioSourcePool.Pool.GetInstance();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        public void ReconstructToDefault(AudioSource audioSource) { }

        public void Construct(AudioSource audioSource)
        {
            audioSource.Play();
        }

        public AudioSource NewInstance() => Instantiate(Prefabs[Random.Range(0, Prefabs.Length)]);
    }
}
