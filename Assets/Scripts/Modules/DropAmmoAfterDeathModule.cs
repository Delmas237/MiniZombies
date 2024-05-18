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
        public IPool<AmmoPack> AmmoPool { get; set; }
        private Transform _transform;

        public void Initialize(IHealthController healthController, Transform transform)
        {
            if (Enabled)
            {
                healthController.Died += DropAmmo;
                _transform = transform;
            }
        }

        private void DropAmmo()
        {
            float rnd = Random.Range(0f, 1f);

            if (rnd < _dropChance)
            {
                AmmoPack ammo = AmmoPool.GetFreeElement();

                Vector3 defaultSpawnPos = _transform.position + 0.7f * Vector3.up;
                ammo.transform.SetPositionAndRotation(defaultSpawnPos, Quaternion.identity);
            }
        }
    }
}
