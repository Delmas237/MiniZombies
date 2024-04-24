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

        protected HealthController healthController;

        public virtual void Initialize(HealthController _healthController)
        {
            healthController = _healthController;

            healthController.Died += OnDeath;
            healthController.Healed += healSound.Play;
            healthController.Damaged += damageSound.Play;
        }
        public virtual void OnDeath()
        {
            deathSound.Play();

            healthController.Died -= OnDeath;
            healthController.Healed -= healSound.Play;
            healthController.Damaged -= damageSound.Play;
        }
    }
}
