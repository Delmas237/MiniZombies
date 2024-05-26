using ObjectPool;
using PlayerLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyLib
{
    public class EnemySpawner : MonoBehaviour
    {
        [field: SerializeField] public bool IsSpawn { get; set; } = true;
        [field: SerializeField] public float Cooldown { get; set; } = 3;

        [SerializeField] private int _maxEnemiesOnScene = 50;
        public int EnemiesOnScene => _enemiesOnScene.Count;
        public int EnemiesDied { get; private set; }

        private readonly List<IEnemy> _enemiesOnScene = new List<IEnemy>();

        [SerializeField] private List<Transform> _spawnDots;

        [SerializeField] private List<ZombiePool> _zombiePools;
        [SerializeField] private List<ZombieShooterPool> _zombieShooterPools;

        [SerializeField] private PlayerContainer _player;
        [SerializeField] private PoolAmmoPack _ammoPackPool;

        private void Start()
        {
            for (int i = 0; i < _zombiePools.Count; i++)
                _zombiePools[i].Initialize(_spawnDots, _player, _ammoPackPool);
            
            for (int i = 0; i < _zombieShooterPools.Count; i++)
                _zombieShooterPools[i].Initialize(_spawnDots, _player, _ammoPackPool);

            StartCoroutine(SpawnController());

            _player.HealthController.Died += StopSpawn;

            void StopSpawn() => IsSpawn = false;
        }

        private IEnumerator SpawnController()
        {
            while (true)
            {
                if (IsSpawn && EnemiesOnScene < _maxEnemiesOnScene)
                    Spawn();

                yield return new WaitForSeconds(Cooldown);
            }
        }

        private void Spawn()
        {
            int rnd = Random.Range(0, _zombieShooterPools.Count);

            IEnemy enemy = _zombieShooterPools[rnd].GetFreeElement();
            _enemiesOnScene.Add(enemy);
            enemy.HealthController.Died += RemoveDiedEnemies;
        }

        private void RemoveDiedEnemies()
        {
            List<IEnemy> enemiesForRemove = new List<IEnemy>();
            foreach (var enemy in _enemiesOnScene)
            {
                if (enemy == null || enemy.HealthController.Health <= 0)
                    enemiesForRemove.Add(enemy);
            }

            foreach (var enemy in enemiesForRemove)
            {
                enemy.HealthController.Died -= RemoveDiedEnemies;
                _enemiesOnScene.Remove(enemy);
                EnemiesDied++;
            }
        }
    }
}
