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
        [SerializeField] private ZombieContainer Enemy;
        [SerializeField] private int amount = 20;
        [SerializeField] private bool autoExpand = true;

        private PoolBase<ZombieContainer> pool;
        private ZombieFactory factory;

        public void Initialize(List<Transform> spawnDots, PlayerContainer player, IPool<AmmoPack> ammoPackPool, Transform parent)
        {
            factory = new ZombieFactory(Enemy, parent, player, spawnDots, ammoPackPool);
            pool = new PoolBase<ZombieContainer>(Enemy, amount, factory)
            {
                AutoExpand = autoExpand
            };
        }

        public ZombieContainer GetFreeElement()
        {
            ZombieContainer enemy = pool.GetFreeElement();

            factory.ReconstructToDefault(enemy);
            factory.Construct(enemy);

            return enemy;
        }
    }
}
