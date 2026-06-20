using EventBusLib;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity.Hostile
{
    public class EnemySpawner : Spawner<IHostile>
    {
        [SerializeField] private List<EnemySpawnData> _spawnData;
        [SerializeField] private PlayerEntity _player;

        public override event Action<IHostile> Spawned;
        public override event Action Removed;

        protected override void Start()
        {
            base.Start();

            foreach (var spawnData in _spawnData)
                spawnData.Initialize(_spawnDots, _player);

            EventBus.Subscribe<WaveStartedEvent>(OnWaveStarted);
            EventBus.Subscribe<GameOverEvent>(StopSpawn);
        }

        private void OnWaveStarted(WaveStartedEvent waveStartedEvent)
        {
            IsSpawn = true;
            _cooldown = waveStartedEvent.Wave.SpawnSpeed;
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
                        IHostile enemy = spawnData.Factory.GetInstance();
                        _objectsOnScene.Add(enemy);
                        Spawned?.Invoke(enemy);

                        enemy.HealthModule.IsOver += RemoveDiedEnemies;
                        return;
                    }
                    current += spawnData.Priority;
                }
            }
        }

        private void RemoveDiedEnemies()
        {
            List<IHostile> enemiesForRemove = new List<IHostile>();
            foreach (var enemy in ObjectsOnScene)
            {
                if (enemy == null || enemy.HealthModule.Health <= 0)
                    enemiesForRemove.Add(enemy);
            }

            foreach (var enemy in enemiesForRemove)
            {
                enemy.HealthModule.IsOver -= RemoveDiedEnemies;
                _objectsOnScene.Remove(enemy);
                Removed?.Invoke();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventBus.Unsubscribe<WaveStartedEvent>(OnWaveStarted);
            EventBus.Unsubscribe<GameOverEvent>(StopSpawn);
        }
    }
}
