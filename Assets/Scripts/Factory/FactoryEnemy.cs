using EnemyLib;
using ObjectPool;
using PlayerLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Factory
{
    public class FactoryEnemy : FactoryBase<EnemyContainer>
    {
        private readonly PlayerContainer player;
        private readonly List<Transform> spawnDots;
        private readonly IPool<AmmoPack> ammoPackPool;

        public FactoryEnemy(EnemyContainer prefab, Transform parent, PlayerContainer player, List<Transform> spawnDots, IPool<AmmoPack> ammoPackPool)
            : base(prefab, parent)
        {
            this.player = player;
            this.spawnDots = spawnDots;
            this.ammoPackPool = ammoPackPool;
        }

        public override void ReconstructToDefault(EnemyContainer enemy)
        {
            enemy.enabled = true;

            if (enemy.TryGetComponent(out Rigidbody rb))
                Object.Destroy(rb);

            if (enemy.MoveController.Agent == null)
                enemy.MoveController.Agent = enemy.GetComponent<NavMeshAgent>();
            else
                enemy.MoveController.Agent.enabled = true;

            if (enemy.TryGetComponent(out CapsuleCollider collider))
            {
                collider.isTrigger = false;
                collider.height = 1.9f;
            }
        }
        public override void Construct(EnemyContainer enemy)
        {
            List<Transform> spawnDotsCopy = new List<Transform>(spawnDots);
            List<Transform> spawnDotsFurthest = new List<Transform>
            {
                SearchFurthest(ref spawnDotsCopy),
                SearchFurthest(ref spawnDotsCopy),
                SearchFurthest(ref spawnDotsCopy)
            };

            Transform randSpawnDot = spawnDotsFurthest[Random.Range(0, spawnDotsFurthest.Count)];
            
            enemy.transform.SetPositionAndRotation(randSpawnDot.position, Quaternion.identity);

            enemy.MoveController.Target = player;
            enemy.HealthController.MaxHealth = 70 + EnemyWaveManager.CurrentWaveIndex * 2;
            enemy.HealthController.Health = enemy.HealthController.MaxHealth;

            float speedX = (float)Math.Round(Random.Range(0.9f, 1.15f) + EnemyWaveManager.CurrentWaveIndex * 0.01f, 2);
            enemy.MoveController.Speed = enemy.MoveController.DefaultSpeed * speedX;
            enemy.MoveController.Agent.speed = enemy.MoveController.Speed;
            enemy.AnimationController.AttackSpeedX = speedX;

            enemy.DropAmmoAfterDeathModule.AmmoPool = ammoPackPool;
        }
        private Transform SearchFurthest(ref List<Transform> spawnDots)
        {
            Transform transform = ComponentSearcher<Transform>.Furthest(player.transform.position, spawnDots);
            spawnDots.Remove(transform);
            return transform;
        }
    }
}
