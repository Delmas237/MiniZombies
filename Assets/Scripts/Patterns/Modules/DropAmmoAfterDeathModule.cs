using EventBusLib;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    [Serializable]
    public class DropAmmoAfterDeathModule : IModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(5)]
        [SerializeField, Range(0f, 1f)] private float _dropChance = 1;

        private Transform _transform;
        private IHealthController _healthController;

        public IInstanceProvider<AmmoPack> AmmoProvider { get; set; }
        public bool Enabled => _enabled;

        public void Initialize(IHealthController healthController, Transform transform)
        {
            if (_enabled)
            {
                _healthController = healthController;
                _transform = transform;

                healthController.IsOver += DropAmmo;
                EventBus.Subscribe<GameExitEvent>(Unsubscribe);
            }
        }
        private void Unsubscribe(GameExitEvent exitEvent)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            _healthController.IsOver -= DropAmmo;
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
