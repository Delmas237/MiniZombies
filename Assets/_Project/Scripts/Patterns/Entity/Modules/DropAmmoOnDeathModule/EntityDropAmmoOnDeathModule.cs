using EventBusLib;
using System;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Entity
{
    [Serializable]
    public class EntityDropAmmoOnDeathModule : IEntityModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(5)]
        [SerializeField, Range(0f, 1f)] private float _dropChance = 1;

        private Transform _transform;
        private IEntityHealthModule _healthModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public IInstanceProvider<AmmoPack> AmmoProvider { get; set; }

        public void Initialize(IEntityHealthModule healthModule, Transform transform)
        {
            if (_enabled)
            {
                _healthModule = healthModule;
                _transform = transform;

                healthModule.IsOver += DropAmmo;
                EventBus.Subscribe<GameExitEvent>(Unsubscribe);
            }
        }
        private void Unsubscribe(GameExitEvent exitEvent)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            _healthModule.IsOver -= DropAmmo;
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
