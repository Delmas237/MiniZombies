using Factory;
using ObjectPool;
using UnityEngine;

public class AmmoPackPoolInitializer : MonoBehaviour
{
    [SerializeField] private AmmoPackPool _ammoPackPool;
    [SerializeField] private AudioSourceFactory _audioSourcePool;

    private void Start()
    {
        foreach (var ammo in _ammoPackPool.Pool.Elements)
            ammo.Intialize(_audioSourcePool);
    }
}
