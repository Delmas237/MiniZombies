using EnemyLib;
using ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Factory
{
    public class ZombieFactory : FactoryBase<ZombieContainer>, IFactory<IEnemy>
    {
        protected IEntity _target;
        protected List<Transform> _spawnDots;
        protected IPool<AmmoPack> _ammoPackPool;

        protected IPool<ZombieContainer> _pool;
        public IPool<ZombieContainer> Pool => _pool;

        public ZombieFactory(IPool<ZombieContainer> pool, IPool<AmmoPack> ammoPackPool, List<Transform> spawnDots, 
            IEntity target) : base(pool.Prefab)
        {
            _pool = pool;
            _ammoPackPool = ammoPackPool;
            _spawnDots = spawnDots;
            _target = target;

            foreach (ZombieContainer enemy in Pool.Pool)
                enemy.MoveController.Speed = enemy.MoveController.DefaultSpeed;

            EnemyWaveManager.WaveFinished += BoostEnemies;
            _target.HealthController.Died += Unsubscribe;
        }
        private void Unsubscribe()
        {
            _target.HealthController.Died -= Unsubscribe;
            EnemyWaveManager.WaveFinished -= BoostEnemies;
        }

        private void BoostEnemies()
        {
            foreach (ZombieContainer enemy in Pool.Pool)
            {
                enemy.HealthController.MaxHealth *= 1.03f;

                float randomX = Random.Range(0.9f, 1.15f);
                float boosterValue = EnemyWaveManager.CurrentWaveIndex * 0.01f;
                float speedX = (float)Math.Round(randomX + boosterValue, 2);
                enemy.MoveController.Speed = enemy.MoveController.DefaultSpeed * speedX;
                enemy.AttackController.AttackSpeed = speedX;
            }
        }

        public virtual IEnemy GetInstance()
        {
            ZombieContainer instance = _pool.GetFreeElement();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        protected override void ReconstructToDefault(ZombieContainer enemy)
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
        protected override void Construct(ZombieContainer enemy)
        {
            enemy.enabled = true;

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
        }
        private Transform SearchFurthest(ref List<Transform> spawnDots)
        {
            Transform transform = ComponentSearcher<Transform>.Furthest(_target.Transform.position, spawnDots);
            spawnDots.Remove(transform);
            return transform;
        }
    }
}
