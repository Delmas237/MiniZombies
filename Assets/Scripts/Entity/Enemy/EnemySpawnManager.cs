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

        [SerializeField] private int _maxEnemiesOnScene = 50;
        public int EnemiesOnScene => _enemiesOnScene.Count;
        public int EnemiesDied { get; private set; }

        [SerializeField] private List<Transform> _spawnDots;

        [SerializeField] private List<ZombiePool> _enemyPools;
        private List<ZombieContainer> _enemiesOnScene = new List<ZombieContainer>();

        [SerializeField] private PlayerContainer _player;
        [SerializeField] private PoolAmmoPack _ammoPackPool;

        private void Start()
        {
            for (int i = 0; i < _enemyPools.Count; i++)
                _enemyPools[i].Initialize(_spawnDots, _player, _ammoPackPool, _enemyPools[i].transform);

            StartCoroutine(SpawnTimer());

            _player.HealthController.Died += SpawnFalse;

            void SpawnFalse() => Spawn = false;
        }

        private void Update()
        {
            EnemiesOnSceneCheck();
        }

        private IEnumerator SpawnTimer()
        {
            while (true)
            {
                if (Spawn && EnemiesOnScene < _maxEnemiesOnScene)
                    SpawnEnemy();

                yield return new WaitForSeconds(Cooldown);
            }
        }

        private void SpawnEnemy()
        {
            if (_player)
            {
                int rnd = Random.Range(0, _enemyPools.Count);

                ZombieContainer enemy = _enemyPools[rnd].GetFreeElement();
                _enemiesOnScene.Add(enemy);
            }
        }

        private void EnemiesOnSceneCheck()
        {
            for (int i = 0; i < _enemiesOnScene.Count; i++)
            {
                if (_enemiesOnScene[i] == null ||
                    _enemiesOnScene[i].enabled == false)
                {
                    _enemiesOnScene.Remove(_enemiesOnScene[i]);
                    EnemiesDied++;
                }
            }
        }
    }
}
