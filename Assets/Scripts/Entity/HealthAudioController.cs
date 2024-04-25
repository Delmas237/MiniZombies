using System;
using UnityEngine;

namespace PlayerLib
{
    [Serializable]
    public class HealthAudioController
    {
        [SerializeField] protected AudioSource damageSound;
        [SerializeField] protected AudioSource healSound;
        [SerializeField] protected AudioSource deathSound;

        public virtual void Initialize(HealthController _healthController)
        {
            _healthController.Died += OnDeath;
            _healthController.Healed += healSound.Play;
            _healthController.Damaged += damageSound.Play;
        }
        public virtual void OnDeath()
        {
            deathSound.Play();
        }
    }
}
