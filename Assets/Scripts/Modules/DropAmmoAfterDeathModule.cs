using EventBusLib;
using ObjectPool;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    [Serializable]
    public class DropAmmoAfterDeathModule : IModule
    {
        [field: SerializeField] public bool Enabled { get; set; } = true;
        [Space(5)]
        [SerializeField, Range(0f, 1f)] private float _dropChance = 1;
        public IInstanceProvider<AmmoPack> AmmoProvider { get; set; }
        private Transform _transform;

        private IHealthController _healthController;

        public void Initialize(IHealthController healthController, Transform transform)
        {
            if (Enabled)
            {
                _healthController = healthController;
                _transform = transform;

                healthController.Died += DropAmmo;
                EventBus.Subscribe<GameExitEvent>(Unsubscribe);
            }
        }
        private void Unsubscribe(GameExitEvent exitEvent)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            _healthController.Died -= DropAmmo;
        }

        private void DropAmmo()
        {
            float rnd = Random.Range(0f, 1f);

            if (rnd < _dropChance)
            {
                AmmoPack ammo = AmmoProvider.GetInstance();

                Vector3 defaultSpawnPos = _transform.position + 0.7f * Vector3.up;
                ammo.transform.SetPositionAndRotation(defaultSpawnPos, Quaternion.identity);
            }
        }
    }
}
