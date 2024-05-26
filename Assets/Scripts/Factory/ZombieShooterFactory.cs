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
    public class ZombieShooterFactory : FactoryBase<ZombieShooterContainer>
    {
        private readonly IPlayer _player;
        private readonly List<Transform> _spawnDots;
        private readonly IPool<AmmoPack> _ammoPackPool;
        private readonly IPool<ParticleSystem> _shotPool;

        public ZombieShooterFactory(ZombieShooterContainer prefab, IPlayer player, List<Transform> spawnDots, 
            IPool<AmmoPack> ammoPackPool, IPool<ParticleSystem> shotPool)
            : base(prefab)
        {
            _player = player;
            _spawnDots = spawnDots;
            _ammoPackPool = ammoPackPool;
            _shotPool = shotPool;
        }

        public override void ReconstructToDefault(ZombieShooterContainer enemy)
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

        public override void Construct(ZombieShooterContainer enemy)
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

            enemy.MoveController.Target = _player;
            enemy.HealthController.MaxHealth = 70 + EnemyWaveManager.CurrentWaveIndex * 2;
            enemy.HealthController.Health = enemy.HealthController.MaxHealth;

            float speedX = (float)Math.Round(Random.Range(0.9f, 1.15f) + EnemyWaveManager.CurrentWaveIndex * 0.01f, 2);
            enemy.MoveController.Speed = enemy.MoveController.DefaultSpeed * speedX;
            enemy.AnimationController.AttackSpeedX = speedX;

            enemy.WeaponsController.ChangeGun(GunType.Pistol);
            
            enemy.DropAmmoAfterDeathModule.AmmoPool = _ammoPackPool;
            enemy.WeaponsController.CurrentGun.ShotPool = _shotPool;

            enemy.enabled = true;
        }

        private Transform SearchFurthest(ref List<Transform> spawnDots)
        {
            Transform transform = ComponentSearcher<Transform>.Furthest(_player.Transform.position, spawnDots);
            spawnDots.Remove(transform);
            return transform;
        }
    }
}
