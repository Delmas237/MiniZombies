using Factory;
using UnityEngine;

namespace ObjectPool
{
    public class PoolAudioSource : MonoBehaviour,  IPool<AudioSource>
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private int _amount = 3;
        [SerializeField] private bool _autoExpand = true;
        [SerializeField] private Transform _parent;

        private PoolBase<AudioSource> _pool;
        private FactoryAudioSource _factory;

        private void Awake()
        {
            if (_parent == null)
                _parent = transform;

            _factory = new FactoryAudioSource(_audioSource);
            _pool = new PoolBase<AudioSource>(_audioSource, _amount, _factory, _parent)
            {
                AutoExpand = _autoExpand
            };
        }

        public AudioSource GetFreeElement()
        {
            AudioSource audioSource = _pool.GetFreeElement();

            _factory.ReconstructToDefault(audioSource);
            _factory.Construct(audioSource);

            return audioSource;
        }
    }
}
