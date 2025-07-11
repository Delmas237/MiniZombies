using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] protected GunType _type;
        [SerializeField] protected GunFireType _fireType;
        [Space(5)]
        [SerializeField] protected Transform _shootDir;
        [SerializeField] protected Transform _muzzle;
        [Space(5)]
        [SerializeField] protected bool _setTrailParent;

        protected float _currentCooldown;
        protected bool _canShoot = true;

        /// <summary>
        /// % of damage reduction on the next penetration
        /// </summary>
        public const float MINUS_PIERCING_DAMAGE = 5;
        /// <summary>
        /// Trail lifetime to cover a distance of 1 unit
        /// </summary>
        public const float TRAIL_UNIT_TIME = 0.05f;

        public event Action Shooted;

        [field: Space(5)]
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public float Cooldown { get; set; }
        [field: SerializeField] public float Distance { get; set; } = 4;
        [field: SerializeField] public int Consumption { get; set; } = 1;
        public IInstanceProvider<BulletTrail> BulletPool { get; set; }

        public GunType Type => _type;
        public GunFireType FireType => _fireType;
        public float CurrentCooldown => _currentCooldown;

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
                BulletTrail bullet = Shoot(_shootDir.position);

                float pitchRange = 0.02f;
                bullet.AudioSource.pitch += Random.Range(-pitchRange, pitchRange);

                _canShoot = false;
                _currentCooldown = Cooldown;
                return true;
            }

            return false;
        }

        protected BulletTrail Shoot(Vector3 vector)
        {
            if (gameObject.activeInHierarchy)
            {
                BulletTrail bullet = BulletPool.GetInstance();

                if (_setTrailParent)
                    bullet.transform.parent = transform;

                bullet.transform.SetPositionAndRotation(_muzzle.position, _muzzle.rotation);

                GunData gunData = GunsDataSaver.GunsData[Type];
                InitializeAudio(gunData, bullet.AudioSource);
                InitializeBulletTrail(gunData, bullet.ParticleSystem);

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
                    entities[i].HealthModule.Decrease(piercingDamage);
                    piercingDamage -= (piercingDamage / 100) * MINUS_PIERCING_DAMAGE;
                }

                Shooted?.Invoke();
                return bullet;
            }
            return null;
        }

        private void InitializeAudio(GunData gunData, AudioSource audio)
        {
            audio.clip = gunData.AudioClip;
            audio.volume = gunData.Volume;
            audio.pitch = gunData.Pitch;
            audio.Play();
        }
        private void InitializeBulletTrail(GunData gunData, ParticleSystem particles)
        {
            var mainModule = particles.main;
            var shape = particles.shape;

            mainModule.startLifetime = TRAIL_UNIT_TIME * Distance;
            shape.angle = gunData.ShapeAngle;
        }
    }
}
