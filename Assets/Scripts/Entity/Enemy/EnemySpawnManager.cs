using ObjectPool;
using PlayerLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyLib
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [field: SerializeField] public bool Spawn { get; set; } = true;
        [field: SerializeField] public float Cooldown { get; set; } = 3;

        [SerializeField] private int maxEnemiesOnScene = 50;
        public int EnemiesOnScene => enemiesOnScene.Count;
        public int EnemiesDied { get; private set; }

        [SerializeField] private List<Transform> spawnDots;

        [SerializeField] private ZombiePool[] enemyPools;
        private List<ZombieContainer> enemiesOnScene = new List<ZombieContainer>();

        [SerializeField] private PlayerContainer player;
        [SerializeField] private PoolAmmoPack ammoPackPool;

        private void Start()
        {
            for (int i = 0; i < enemyPools.Length; i++)
                enemyPools[i].Initialize(spawnDots, player, ammoPackPool, enemyPools[i].transform);

            StartCoroutine(SpawnTimer());

            player.HealthController.Died += SpawnFalse;

            void SpawnFalse() => Spawn = false;
        }

        private void Update()
        {
            EnemiesOnSceneCheck();
        }

        private IEnumerator SpawnTimer()
        {
            if (Spawn && EnemiesOnScene < maxEnemiesOnScene)
                SpawnEnemy();

            yield return new WaitForSeconds(Cooldown);

            StartCoroutine(SpawnTimer());
        }

        private void SpawnEnemy()
        {
            if (player)
            {
                int rnd = Random.Range(0, enemyPools.Length);

                ZombieContainer enemy = enemyPools[rnd].GetFreeElement();
                enemiesOnScene.Add(enemy);
            }
        }

        private void EnemiesOnSceneCheck()
        {
            for (int i = 0; i < enemiesOnScene.Count; i++)
            {
                if (enemiesOnScene[i] == null ||
                    enemiesOnScene[i].enabled == false)
                {
                    enemiesOnScene.Remove(enemiesOnScene[i]);
                    EnemiesDied++;
                }
            }
        }
    }
}
