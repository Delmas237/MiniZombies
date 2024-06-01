using Factory;
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
        public int EnemiesDied { get; private set; }

        private static readonly List<IEnemy> _enemiesOnScene = new List<IEnemy>();
        public static IReadOnlyList<IEnemy> EnemiesOnScene => _enemiesOnScene;

        [SerializeField] private List<Transform> _spawnDots;

        [SerializeField] private List<EnemyPool> _enemyPools;
        private readonly List<IFactory<IEnemy>> _factories = new List<IFactory<IEnemy>>();

        [SerializeField] private PlayerContainer _player;
        [SerializeField] private AmmoPackPool _ammoPackPool;

        [SerializeField] private ParticleSystemPool _pistolShotPool;

        private void Start()
        {
            foreach (var pool in _enemyPools)
            {
                if (pool.Pool is IPool<ZombieShooterContainer> shooterPool)
                {
                    _factories.Add(new ZombieShooterFactory(shooterPool, _ammoPackPool.Pool, _spawnDots, _player, _pistolShotPool.Pool));
                }
                else
                {
                    _factories.Add(new ZombieFactory(pool.Pool, _ammoPackPool.Pool, _spawnDots, _player));
                }
            }

            StartCoroutine(SpawnController());

            _player.HealthController.Died += StopSpawn;
        }

        private void StopSpawn()
        {
            _player.HealthController.Died -= StopSpawn;
            IsSpawn = false;
        }

        private void OnDestroy()
        {
            _enemiesOnScene.Clear();
        }

        private IEnumerator SpawnController()
        {
            while (true)
            {
                if (IsSpawn && _enemiesOnScene.Count < _maxEnemiesOnScene)
                    Spawn();

                yield return new WaitForSeconds(Cooldown);
            }
        }

        private void Spawn()
        {
            int rnd = Random.Range(0, _factories.Count);

            IEnemy enemy = _factories[rnd].GetInstance();
            _enemiesOnScene.Add(enemy);
            enemy.HealthController.Died += RemoveDiedEnemies;
        }

        private void RemoveDiedEnemies()
        {
            List<IEnemy> enemiesForRemove = new List<IEnemy>();
            foreach (var enemy in EnemiesOnScene)
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
