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
        protected IPool<AmmoPack> _ammoPackPool;

        protected IPool<ZombieContainer> _pool;
        public IPool<ZombieContainer> Pool => _pool;

        private ZombieContainer _prefab;
        public ZombieContainer Prefab => _prefab;

        private EnemyWaveBoostData _waveBoostData;

        public ZombieFactory(IPool<ZombieContainer> pool, IPool<AmmoPack> ammoPackPool, List<Transform> spawnDots, 
            IEntity target, EnemyWaveBoostData waveBoostData)
        {
            _prefab = pool.Prefab;
            _pool = pool;
            _ammoPackPool = ammoPackPool;
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
            enemy.MoveController.Speed = enemy.MoveController.DefaultSpeed;
            enemy.AttackController.Speed = enemy.AttackController.DefaultSpeed;
        }

        private void BoostEnemies(WaveFinishedEvent waveFinishedEvent)
        {
            foreach (ZombieContainer enemy in Pool.Elements)
            {
                enemy.HealthController.MaxHealth *= 1 + _waveBoostData.HpPercent;

                float randomX = Random.Range(0.9f, 1.15f);
                float boosterValue = waveFinishedEvent.Number * _waveBoostData.WaveMultiplierSpeed;
                float speedX = (float)Math.Round(randomX + boosterValue, 2);
                enemy.MoveController.Speed = enemy.MoveController.DefaultSpeed * speedX;
                enemy.MoveController.Agent.speed = speedX;
                enemy.AttackController.Speed = speedX;
            }
        }

        public virtual IEnemy GetInstance()
        {
            ZombieContainer instance = _pool.GetFreeElement();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

       public void ReconstructToDefault(ZombieContainer enemy)
        {
            if (enemy.TryGetComponent(out Rigidbody rb))
                Object.Destroy(rb);

            if (enemy.MoveController.Agent != null)
                enemy.MoveController.Agent.enabled = true;

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

            enemy.MoveController.Target = _target;
            enemy.HealthController.Health = enemy.HealthController.MaxHealth;

            enemy.DropAmmoAfterDeathModule.AmmoPool = _ammoPackPool;

            enemy.enabled = true;
        }
        private Transform SearchFurthest(ref List<Transform> spawnDots)
        {
            Transform transform = ComponentSearcher<Transform>.Furthest(_target.Transform.position, spawnDots);
            spawnDots.Remove(transform);
            return transform;
        }

        public ZombieContainer NewInstance() => Object.Instantiate(Prefab);
    }
}
