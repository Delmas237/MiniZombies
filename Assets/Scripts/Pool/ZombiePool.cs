using EnemyLib;
using Factory;
using PlayerLib;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace ObjectPool
{
    public class ZombiePool : MonoBehaviour, IPool<ZombieContainer>
    {
        [SerializeField] private ZombieContainer _zombie;
        [SerializeField] private int _amount = 20;
        [SerializeField] private bool _autoExpand = true;

        private PoolBase<ZombieContainer> _pool;
        private ZombieFactory _factory;

        public void Initialize(List<Transform> spawnDots, PlayerContainer player, IPool<AmmoPack> ammoPackPool, Transform parent)
        {
            _factory = new ZombieFactory(_zombie, parent, player, spawnDots, ammoPackPool);
            _pool = new PoolBase<ZombieContainer>(_zombie, _amount, _factory)
            {
                AutoExpand = _autoExpand
            };
        }

        public ZombieContainer GetFreeElement()
        {
            ZombieContainer enemy = _pool.GetFreeElement();

            _factory.ReconstructToDefault(enemy);
            _factory.Construct(enemy);

            return enemy;
        }
    }
}
