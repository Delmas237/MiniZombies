using System;
using UnityEngine;

[Serializable]
public class HealthAudioController
{
    [SerializeField] protected AudioSource _damageSound;
    [SerializeField] protected AudioSource _healSound;
    [SerializeField] protected AudioSource _deathSound;

    public virtual void Initialize(IHealthController _healthController)
    {
        _healthController.Died += OnDeath;
        _healthController.Healed += _healSound.Play;
        _healthController.Damaged += _damageSound.Play;
    }
    public virtual void OnDeath()
    {
        _deathSound.Play();
    }
}
