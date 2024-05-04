using ObjectPool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapons
{
    public class Gun : MonoBehaviour
    {
        [field: SerializeField] public GunType Type { get; private set; }
        [field: SerializeField] public GunFireType FireType { get; private set; }
        [field: Space(5)]
        [field: SerializeField] public float Damage { get; set; }

        [field: SerializeField] public float Cooldown { get; set; }
        
        protected float currentCooldown;
        public float CurrentCooldown => currentCooldown;
        protected bool canShoot = true;

        [field: SerializeField] public float Distance { get; set; } = 4;
        [field: SerializeField] public int Consumption { get; set; } = 1;

        /// <summary>
        /// % of damage reduction on the next penetration
        /// </summary>
        public const float MinusPiercingDamage = 5;

        protected LayerMask layerMask = 183; //all without player
        [Space(5)]
        [SerializeField] protected Transform shootDir;
        [SerializeField] protected Transform muzzle;

        [SerializeField] protected PoolParticleSystem shotPool;
        protected AudioSource shotSound;

        protected virtual void Update()
        {
            CooldownCheck();
        }

        protected virtual void CooldownCheck()
        {
            Utilities.Timer(ref currentCooldown, delegate ()
            {
                canShoot = true;
            });
        }

        public virtual bool ShootRequest()
        {
            if (canShoot)
            {
                Shoot(shootDir.position);

                float randomRange = 0.02f;
                SoundPitch(shotSound.pitch = Random.Range(shotSound.pitch - randomRange, shotSound.pitch + randomRange));

                canShoot = false;
                currentCooldown = Cooldown;
                return true;
            }

            return false;
        }

        protected void Shoot(Vector3 vector)
        {
            if (gameObject.activeInHierarchy)
            {
                GameObject shot = shotPool.GetFreeElement().gameObject;
                shotSound = shot.GetComponent<AudioSource>();
                shot.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);

                Ray ray = new Ray(vector, shootDir.forward);
                RaycastHit[] hits = Physics.RaycastAll(ray, Distance, layerMask).OrderBy(hit => hit.distance).ToArray();

                List<IEnemy> enemies = new List<IEnemy>();
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject.TryGetComponent(out IEnemy enemy))
                        enemies.Add(enemy);
                }

                float piercingDamage = Damage;
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].HealthController.Health -= piercingDamage;
                    piercingDamage -= (piercingDamage / 100) * MinusPiercingDamage;
                }
            }
        }

        protected void SoundPitch(float pitch) => shotSound.pitch = pitch;
    }
}
