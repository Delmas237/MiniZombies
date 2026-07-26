using Entity;
using Entity.Hostile;
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
    public class ZombieFactory : IFactory<EntityBase>, IInstanceProvider<IEntity>
    {
        protected readonly IEntity _target;
        protected readonly List<Transform> _spawnDots;
        protected readonly IInstanceProvider<AmmoPack> _ammoPackProvider;
        protected readonly IPool<EntityBase> _pool;

        protected readonly EntityBase[] _prefabs;
        protected readonly EnemyWaveBoostData _waveBoostData;

        public IPool<EntityBase> Pool => _pool;
        public EntityBase[] Prefabs => _prefabs;

        public ZombieFactory(IPool<EntityBase> pool, IInstanceProvider<AmmoPack> ammoPackProvider, List<Transform> spawnDots, 
            IEntity target, EnemyWaveBoostData waveBoostData)
        {
            _prefabs = pool.Prefabs;
            _pool = pool;
            _ammoPackProvider = ammoPackProvider;
            _spawnDots = spawnDots;
            _target = target;
            _waveBoostData = waveBoostData;

            foreach (EntityBase enemy in Pool.Elements)
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
        
        private void InitializeEnemy(EntityBase enemy)
        {
            var movementModule = enemy.GetModule<IEnemyMovementModule>();
            var attackModule = enemy.GetModule<IEnemyAttackModule>();

            movementModule.Speed = movementModule.DefaultSpeed;
            attackModule.Speed = attackModule.DefaultSpeed;
        }

        private void BoostEnemies(WaveFinishedEvent waveFinishedEvent)
        {
            foreach (EntityBase enemy in Pool.Elements)
            {
                var healthModule = enemy.HealthModule;
                var movementModule = enemy.GetModule<IEnemyMovementModule>();
                var attackModule = enemy.GetModule<IEnemyAttackModule>();

                healthModule.MaxHealth *= (1 + _waveBoostData.HpPercent);

                float randomX = Random.Range(0.9f, 1.15f);
                float boosterValue = waveFinishedEvent.Number * _waveBoostData.WaveMultiplierSpeed;
                float speedX = (float)Math.Round(randomX + boosterValue, 2);
                
                movementModule.Speed = movementModule.DefaultSpeed * speedX;
                attackModule.Speed = speedX;
            }
        }

        public virtual IEntity GetInstance()
        {
            EntityBase instance = _pool.GetInstance();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        public void ReconstructToDefault(EntityBase enemy)
        {
            enemy.SetAllModulesInitialState();
            
            if (enemy.TryGetComponent(out Rigidbody rb))
                Object.Destroy(rb);

            var movementModule = enemy.GetModule<IEnemyMovementModule>();
            if (movementModule.Agent != null)
                movementModule.Agent.enabled = true;

            if (enemy.TryGetComponent(out CapsuleCollider collider))
            {
                collider.isTrigger = false;
                collider.height = 1.9f;
            }
        }
        public virtual void Construct(EntityBase enemy)
        {
            Transform randSpawnDot = GetSpawnPosition();
            enemy.transform.SetPositionAndRotation(randSpawnDot.position, Quaternion.identity);

            var healthModule = enemy.GetModule<IEntityHealthModule>();
            var targetModule = enemy.GetModule<IEntityTargetModule>();
            var dropAmmoOnDeathModule = enemy.GetModule<IEntityDropAmmoOnDeathModule>();

            targetModule.Target = _target;
            healthModule.Increase(healthModule.MaxHealth);
            dropAmmoOnDeathModule.AmmoProvider = _ammoPackProvider;

            enemy.enabled = true;
        }

        private Transform GetSpawnPosition()
        {
            List<Transform> spawnPositionsCopy = new List<Transform>(_spawnDots);
            List<Transform> spawnPositionsFurthest = new List<Transform>
            {
                SearchFurthest(ref spawnPositionsCopy),
                SearchFurthest(ref spawnPositionsCopy),
                SearchFurthest(ref spawnPositionsCopy)
            };

            Transform spawnPosition = spawnPositionsFurthest[Random.Range(0, spawnPositionsFurthest.Count)];
            return spawnPosition;
        }

        private Transform SearchFurthest(ref List<Transform> spawnDots)
        {
            Transform transform = ComponentSearcher<Transform>.Furthest(_target.Transform.position, spawnDots);
            spawnDots.Remove(transform);
            return transform;
        }

        public EntityBase NewInstance() => Object.Instantiate(Prefabs[Random.Range(0, Prefabs.Length)]);
    }
}
