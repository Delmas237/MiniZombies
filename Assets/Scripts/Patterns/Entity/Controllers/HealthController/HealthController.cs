using System;
using UnityEngine;

[Serializable]
public class HealthController : IHealthController
{
    [SerializeField] private float _maxHealth = 100;
    public float MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            if (value <= 0)
            {
                Debug.LogWarning("Max health cannot be less than zero");
                return;
            }
            _maxHealth = value;
            _health = Math.Min(_maxHealth, _health);
        }
    }

    private float _health;
    public float Health => _health;

    public event Action Decreased;
    public event Action Increased;
    public event Action IsOver;

    [field: SerializeField] public HealthAudioController AudioController { get; set; }

    public void Initialize()
    {
        AudioController.Initialize(this);
        _health = MaxHealth;
    }

    public void Decrease(float value)
    {
        if (value <= 0 || _health == 0)
            return;

        _health = Math.Max(_health - value, 0);
        Decreased?.Invoke();

        if (_health <= 0)
            IsOver?.Invoke();
    }

    public void Increase(float value)
    {
        if (value <= 0 || _health == MaxHealth)
            return;

        _health = Math.Min(_health + value, MaxHealth);
        Increased?.Invoke();
    }
}
