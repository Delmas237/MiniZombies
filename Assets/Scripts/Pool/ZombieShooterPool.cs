using EnemyLib;
using Factory;
using PlayerLib;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace ObjectPool
{
    public class ZombieShooterPool : MonoBehaviour, IPool<ZombieShooterContainer>
    {
        [SerializeField] private ZombieShooterContainer _zombie;
        [SerializeField] private int _amount = 20;
        [SerializeField] private bool _autoExpand = true;
        [SerializeField] private Transform _parent;
        [Space(10)]
        [SerializeField] private PoolParticleSystem _poolParticleSystem;

        private PoolBase<ZombieShooterContainer> _pool;
        private ZombieShooterFactory _factory;

        public void Initialize(List<Transform> spawnDots, IPlayer player, IPool<AmmoPack> ammoPackPool)
        {
            if (_parent == null)
                _parent = transform;

            _factory = new ZombieShooterFactory(_zombie, player, spawnDots, ammoPackPool, _poolParticleSystem);
            _pool = new PoolBase<ZombieShooterContainer>(_zombie, _amount, _factory, _parent)
            {
                AutoExpand = _autoExpand
            };
        }

        public ZombieShooterContainer GetFreeElement()
        {
            ZombieShooterContainer enemy = _pool.GetFreeElement();

            _factory.ReconstructToDefault(enemy);
            _factory.Construct(enemy);

            return enemy;
        }
    }
}
