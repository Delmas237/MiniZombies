using EventBusLib;
using PlayerLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private List<EnemySpawnData> _spawnData;
        [SerializeField] private List<Transform> _spawnDots;

        [SerializeField] private PlayerContainer _player;

        private void Start()
        {
            foreach (var spawnData in _spawnData)
                spawnData.Initialize(_spawnDots, _player);

            StartCoroutine(SpawnController());

            EventBus.Subscribe<GameOverEvent>(StopSpawn);
        }
        private void StopSpawn(IEvent e) => IsSpawn = false;

        private void OnDestroy()
        {
            EventBus.Unsubscribe<GameOverEvent>(StopSpawn);
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
            int prioritySum = (int)_spawnData.Sum(p => p.Priority);
            if (prioritySum > 0)
            {
                int random = Random.Range(0, prioritySum);

                float current = 0;
                foreach (var spawnData in _spawnData)
                {
                    if (spawnData.Priority <= 0)
                        continue;

                    if (random >= current && random <= spawnData.Priority + current)
                    {
                        IEnemy enemy = spawnData.Factory.GetInstance();
                        _enemiesOnScene.Add(enemy);
                        enemy.HealthController.Died += RemoveDiedEnemies;
                        return;
                    }
                    current += spawnData.Priority;
                }
            }
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
