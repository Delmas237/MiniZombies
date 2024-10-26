using EventBusLib;
using PlayerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyLib
{
    public class EnemySpawner : Spawner<IEnemy>
    {
        public override event Action<IEnemy> Spawned;
        public override event Action Removed;

        [SerializeField] private List<EnemySpawnData> _spawnData;
        [SerializeField] private PlayerContainer _player;

        protected override void Start()
        {
            base.Start();

            foreach (var spawnData in _spawnData)
                spawnData.Initialize(_spawnDots, _player);

            EventBus.Subscribe<WaveStartedEvent>(OnWaveStarted);
            EventBus.Subscribe<GameOverEvent>(StopSpawn);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventBus.Unsubscribe<WaveStartedEvent>(OnWaveStarted);
            EventBus.Unsubscribe<GameOverEvent>(StopSpawn);
        }
        private void OnWaveStarted(WaveStartedEvent waveStartedEvent)
        {
            IsSpawn = true;
            Cooldown = waveStartedEvent.Wave.SpawnSpeed;
        }
        private void StopSpawn(IEvent e) => IsSpawn = false;

        protected override void Spawn()
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
                        _objectsOnScene.Add(enemy);
                        Spawned?.Invoke(enemy);

                        enemy.HealthController.IsOver += RemoveDiedEnemies;
                        return;
                    }
                    current += spawnData.Priority;
                }
            }
        }

        private void RemoveDiedEnemies()
        {
            List<IEnemy> enemiesForRemove = new List<IEnemy>();
            foreach (var enemy in ObjectsOnScene)
            {
                if (enemy == null || enemy.HealthController.Health <= 0)
                    enemiesForRemove.Add(enemy);
            }

            foreach (var enemy in enemiesForRemove)
            {
                enemy.HealthController.IsOver -= RemoveDiedEnemies;
                _objectsOnScene.Remove(enemy);
                Removed?.Invoke();
            }
        }
    }
}
