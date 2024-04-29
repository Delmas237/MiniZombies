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
        [SerializeField, Range(0f, 1f)] private float dropChance = 1;
        public IPool<AmmoPack> AmmoPool { get; set; }
        private Transform transform;

        public void Initialize(IEntity _entity, Transform _transform)
        {
            if (Enabled)
            {
                _entity.HealthController.Died += DropAmmo;
                transform = _transform;
            }
        }

        private void DropAmmo()
        {
            float rnd = Random.Range(0f, 1f);

            if (rnd < dropChance)
            {
                AmmoPack ammo = AmmoPool.GetFreeElement();

                Vector3 defaultSpawnPos = transform.position + 0.7f * Vector3.up;
                ammo.transform.SetPositionAndRotation(defaultSpawnPos, Quaternion.identity);
            }
        }
    }
}
