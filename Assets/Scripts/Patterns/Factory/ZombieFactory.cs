using EnemyLib;
using EventBusLib;
using ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Factory
{
    public class ZombieFactory : IFactory<ZombieContainer>, IInstanceProvider<IEnemy>
    {
        protected IEntity _target;
        protected List<Transform> _spawnDots;
        protected IInstanceProvider<AmmoPack> _ammoPackProvider;
        protected IPool<ZombieContainer> _pool;

        private readonly ZombieContainer[] _prefabs;
        private readonly EnemyWaveBoostData _waveBoostData;

        public IPool<ZombieContainer> Pool => _pool;
        public ZombieContainer[] Prefabs => _prefabs;

        public ZombieFactory(IPool<ZombieContainer> pool, IInstanceProvider<AmmoPack> ammoPackProvider, List<Transform> spawnDots, 
            IEntity target, EnemyWaveBoostData waveBoostData)
        {
            _prefabs = pool.Prefabs;
            _pool = pool;
            _ammoPackProvider = ammoPackProvider;
            _spawnDots = spawnDots;
            _target = target;
            _waveBoostData = waveBoostData;

            foreach (ZombieContainer enemy in Pool.Elements)
                InitializeEnemy(enemy);

            _pool.Expanded += InitializeEnemy;
            EventBus.Subscribe<WaveFinishedEvent>(BoostEnemies);
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent gameOverEvent)
        {
            _pool.Expanded -= InitializeEnemy;
            EventBus.Unsubscribe<WaveFinishedEvent>(BoostEnemies);
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
        }
        
        private void InitializeEnemy(ZombieContainer enemy)
        {
            enemy.MoveModule.Speed = enemy.MoveModule.DefaultSpeed;
            enemy.AttackModule.Speed = enemy.AttackModule.DefaultSpeed;
        }

        private void BoostEnemies(WaveFinishedEvent waveFinishedEvent)
        {
            foreach (ZombieContainer enemy in Pool.Elements)
            {
                enemy.HealthModule.MaxHealth = enemy.HealthModule.MaxHealth * (1 + _waveBoostData.HpPercent);

                float randomX = Random.Range(0.9f, 1.15f);
                float boosterValue = waveFinishedEvent.Number * _waveBoostData.WaveMultiplierSpeed;
                float speedX = (float)Math.Round(randomX + boosterValue, 2);
                enemy.MoveModule.Speed = enemy.MoveModule.DefaultSpeed * speedX;
                enemy.MoveModule.Agent.speed = speedX;
                enemy.AttackModule.Speed = speedX;
            }
        }

        public virtual IEnemy GetInstance()
        {
            ZombieContainer instance = _pool.GetInstance();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

       public void ReconstructToDefault(ZombieContainer enemy)
        {
            if (enemy.TryGetComponent(out Rigidbody rb))
                Object.Destroy(rb);

            if (enemy.MoveModule.Agent != null)
                enemy.MoveModule.Agent.enabled = true;

            if (enemy.TryGetComponent(out CapsuleCollider collider))
            {
                collider.isTrigger = false;
                collider.height = 1.9f;
            }
        }
        public void Construct(ZombieContainer enemy)
        {
            List<Transform> spawnDotsCopy = new List<Transform>(_spawnDots);
            List<Transform> spawnDotsFurthest = new List<Transform>
            {
                SearchFurthest(ref spawnDotsCopy),
                SearchFurthest(ref spawnDotsCopy),
                SearchFurthest(ref spawnDotsCopy)
            };

            Transform randSpawnDot = spawnDotsFurthest[Random.Range(0, spawnDotsFurthest.Count)];
            enemy.transform.SetPositionAndRotation(randSpawnDot.position, Quaternion.identity);

            enemy.MoveModule.Target = _target;
            enemy.HealthModule.Increase(enemy.HealthModule.MaxHealth);

            enemy.DropAmmoAfterDeathModule.AmmoProvider = _ammoPackProvider;

            enemy.enabled = true;
        }
        private Transform SearchFurthest(ref List<Transform> spawnDots)
        {
            Transform transform = ComponentSearcher<Transform>.Furthest(_target.Transform.position, spawnDots);
            spawnDots.Remove(transform);
            return transform;
        }

        public ZombieContainer NewInstance() => Object.Instantiate(Prefabs[Random.Range(0, Prefabs.Length)]);
    }
}
