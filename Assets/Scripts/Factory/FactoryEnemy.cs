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
    public class FactoryEnemy : FactoryBase<Enemy>
    {
        private readonly Player player;
        private readonly List<Transform> spawnDots;
        private readonly IPool<AmmoPack> ammoPackPool;

        public FactoryEnemy(Enemy prefab, Transform parent, Player player, List<Transform> spawnDots, IPool<AmmoPack> ammoPackPool)
            : base(prefab, parent)
        {
            this.player = player;
            this.spawnDots = spawnDots;
            this.ammoPackPool = ammoPackPool;
        }

        public override void ReconstructToDefault(Enemy enemy)
        {
            enemy.enabled = true;

            if (enemy.TryGetComponent(out Rigidbody rb))
                Object.Destroy(rb);

            if (enemy.Agent == null)
                enemy.Agent = enemy.GetComponent<NavMeshAgent>();
            else
                enemy.Agent.enabled = true;

            if (enemy.TryGetComponent(out CapsuleCollider collider))
            {
                collider.isTrigger = false;
                collider.height = 1.9f;
            }
        }
        public override void Construct(Enemy enemy)
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
            enemy.HealthController.Health = 100;

            float speedX = (float)Math.Round(Random.Range(0.9f, 1.15f), 2);
            enemy.MoveController.Speed *= speedX;
            enemy.Agent.speed = enemy.MoveController.Speed;
            enemy.AnimationController.AttackSpeedX *= speedX;

            if (enemy.TryGetComponent(out DropAmmoAfterDeath dropAmmoAfterDeath))
                dropAmmoAfterDeath.AmmoPool = ammoPackPool;
        }
        private Transform SearchFurthest(ref List<Transform> spawnDots)
        {
            Transform transform = ComponentSearcher<Transform>.Furthest(player.transform.position, spawnDots);
            spawnDots.Remove(transform);
            return transform;
        }
    }
}
