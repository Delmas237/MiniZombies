using EnemyLib;
using ObjectPool;
using UnityEngine;

public class ZombieThrowerPool : EnemyPool
{
    [SerializeField] private PoolBase<ZombieThrowerContainer> _throwerPool;
    public override IPool<ZombieContainer> Pool => _throwerPool;

    private void Awake()
    {
        if (_throwerPool.Parent == null)
            _throwerPool.Parent = transform;

        _throwerPool.Initialize();
    }
}
