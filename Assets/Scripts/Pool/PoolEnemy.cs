using EnemyLib;
using Factory;
using PlayerLib;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace ObjectPool
{
    public class PoolEnemy : MonoBehaviour, IPool<Enemy>
    {
        [SerializeField] private Enemy Enemy;
        [SerializeField] private int amount = 20;
        [SerializeField] private bool autoExpand = true;

        private PoolBase<Enemy> pool;
        private FactoryEnemy factory;

        public void Initialize(List<Transform> spawnDots, Player player, IPool<AmmoPack> ammoPackPool, Transform parent)
        {
            factory = new FactoryEnemy(Enemy, parent, player, spawnDots, ammoPackPool);
            pool = new PoolBase<Enemy>(Enemy, amount, factory)
            {
                AutoExpand = autoExpand
            };
        }

        public Enemy GetFreeElement()
        {
            Enemy enemy = pool.GetFreeElement();

            factory.ReconstructToDefault(enemy);
            factory.Construct(enemy);

            return enemy;
        }
    }
}
