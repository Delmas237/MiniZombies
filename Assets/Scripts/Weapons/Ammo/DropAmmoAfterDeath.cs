using EnemyLib;
using ObjectPool;
using UnityEngine;

namespace Weapons
{
    public class DropAmmoAfterDeath : MonoBehaviour
    {
        [SerializeField] private float dropChance = 1;
        public IPool<AmmoPack> AmmoPool { get; set; }

        private Enemy enemy;

        private void Start()
        {
            enemy = GetComponent<Enemy>();
            enemy.HealthController.Died += DropAmmo;
        }
        private void OnDestroy()
        {
            enemy.HealthController.Died -= DropAmmo;
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
