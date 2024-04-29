using EnemyLib;
using Factory;
using PlayerLib;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace ObjectPool
{
    public class PoolEnemy : MonoBehaviour, IPool<EnemyContainer>
    {
        [SerializeField] private EnemyContainer Enemy;
        [SerializeField] private int amount = 20;
        [SerializeField] private bool autoExpand = true;

        private PoolBase<EnemyContainer> pool;
        private FactoryEnemy factory;

        public void Initialize(List<Transform> spawnDots, PlayerContainer player, IPool<AmmoPack> ammoPackPool, Transform parent)
        {
            factory = new FactoryEnemy(Enemy, parent, player, spawnDots, ammoPackPool);
            pool = new PoolBase<EnemyContainer>(Enemy, amount, factory)
            {
                AutoExpand = autoExpand
            };
        }

        public EnemyContainer GetFreeElement()
        {
            EnemyContainer enemy = pool.GetFreeElement();

            factory.ReconstructToDefault(enemy);
            factory.Construct(enemy);

            return enemy;
        }
    }
}
