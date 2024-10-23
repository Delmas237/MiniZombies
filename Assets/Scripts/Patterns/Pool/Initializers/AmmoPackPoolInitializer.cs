using Factory;
using UnityEngine;
using Weapons;

namespace ObjectPool
{
    public class AmmoPackPoolInitializer : MonoBehaviour
    {
        [SerializeField] private AmmoPackPool _ammoPackPool;
        [SerializeField] private AudioSourceFactory _audioSourceFactory;

        private void Start()
        {
            foreach (var ammo in _ammoPackPool.Pool.Elements)
                ammo.DestroySoundFactory = _audioSourceFactory;

            _ammoPackPool.Pool.Expanded += OnExpanded;
        }
        private void OnDestroy()
        {
            _ammoPackPool.Pool.Expanded -= OnExpanded;
        }

        private void OnExpanded(AmmoPack ammo)
        {
            ammo.DestroySoundFactory = _audioSourceFactory;
        }
    }
}
