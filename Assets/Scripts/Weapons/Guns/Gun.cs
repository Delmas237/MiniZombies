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
        
        protected float _currentCooldown;
        public float CurrentCooldown => _currentCooldown;
        protected bool _canShoot = true;

        [field: SerializeField] public float Distance { get; set; } = 4;
        [field: SerializeField] public int Consumption { get; set; } = 1;

        /// <summary>
        /// % of damage reduction on the next penetration
        /// </summary>
        public const float MINUS_PIERCING_DAMAGE = 5;

        [Space(5)]
        [SerializeField] protected Transform _shootDir;
        [SerializeField] protected Transform _muzzle;

        [SerializeField] protected PoolParticleSystem _shotPool;
        protected AudioSource _shotSound;

        protected virtual void Update()
        {
            CooldownCheck();
        }

        protected virtual void CooldownCheck()
        {
            Utilities.Timer(ref _currentCooldown, delegate ()
            {
                _canShoot = true;
            });
        }

        public virtual bool ShootRequest()
        {
            if (_canShoot)
            {
                Shoot(_shootDir.position);

                float randomRange = 0.02f;
                SoundPitch(_shotSound.pitch = Random.Range(_shotSound.pitch - randomRange, _shotSound.pitch + randomRange));

                _canShoot = false;
                _currentCooldown = Cooldown;
                return true;
            }

            return false;
        }

        protected void Shoot(Vector3 vector)
        {
            if (gameObject.activeInHierarchy)
            {
                GameObject shot = _shotPool.GetFreeElement().gameObject;
                _shotSound = shot.GetComponent<AudioSource>();
                shot.transform.SetPositionAndRotation(_muzzle.position, _muzzle.rotation);

                Ray ray = new Ray(vector, _shootDir.forward);
                RaycastHit[] hits = Physics.RaycastAll(ray, Distance).OrderBy(hit => hit.distance).ToArray();

                List<IEntity> entities = new List<IEntity>();
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject.TryGetComponent(out IEntity entity))
                        entities.Add(entity);
                }

                float piercingDamage = Damage;
                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].HealthController.Health -= piercingDamage;
                    piercingDamage -= (piercingDamage / 100) * MINUS_PIERCING_DAMAGE;
                }
            }
        }

        protected void SoundPitch(float pitch) => _shotSound.pitch = pitch;
    }
}
