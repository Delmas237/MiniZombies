using Factory;
using UnityEngine;

namespace ObjectPool
{
    public class PoolAudioSource : MonoBehaviour,  IPool<AudioSource>
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private int _amount = 3;
        [SerializeField] private bool _autoExpand = true;

        private PoolBase<AudioSource> _pool;
        private FactoryAudioSource _factory;

        private void Awake()
        {
            _factory = new FactoryAudioSource(_audioSource, transform);
            _pool = new PoolBase<AudioSource>(_audioSource, _amount, _factory)
            {
                AutoExpand = _autoExpand
            };
        }

        public AudioSource GetFreeElement()
        {
            AudioSource audioSource = _pool.GetFreeElement();
            _factory.Construct(audioSource);
            return audioSource;
        }
    }
}
