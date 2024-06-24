using EventBusLib;
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

    private IHealthController _healthController;

    public virtual void Initialize(IHealthController healthController)
    {
        _healthController = healthController;

        _healthController.Died += OnDeath;
        _healthController.Healed += OnHeal;
        _healthController.Damaged += OnDamage;

        EventBus.Subscribe<GameExitEvent>(Unsubscribe);
    }
    private void Unsubscribe(GameExitEvent exitEvent)
    {
        EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
        _healthController.Died -= OnDeath;
        _healthController.Healed -= OnHeal;
        _healthController.Damaged -= OnDamage;
    }

    private void OnDeath()
    {
        if (_deathSound != null)
        {
            _deathSound.pitch = Random.Range(_deathSound.pitch - _deathPitchRandomRange, _deathSound.pitch + _deathPitchRandomRange);
            _deathSound.Play();
        }
    }
    private void OnHeal()
    {
        if (_healSound != null)
            _healSound.Play();
    }
    private void OnDamage()
    {
        if (_damageSound != null)
            _damageSound.Play();
    }
}
