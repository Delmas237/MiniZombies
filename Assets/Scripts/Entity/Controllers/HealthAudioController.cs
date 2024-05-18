using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class HealthAudioController
{
    [SerializeField] protected AudioSource _damageSound;
    [SerializeField] protected AudioSource _healSound;
    [SerializeField] protected AudioSource _deathSound;
    [Space(10)]
    [SerializeField] protected float _deathPitchRandomRange;

    public virtual void Initialize(IHealthController _healthController)
    {
        _healthController.Died += OnDeath;
        _healthController.Healed += _healSound.Play;
        _healthController.Damaged += _damageSound.Play;
    }
    public virtual void OnDeath()
    {
        _deathSound.pitch = Random.Range(_deathSound.pitch - _deathPitchRandomRange, _deathSound.pitch + _deathPitchRandomRange);
        _deathSound.Play();
    }
}
