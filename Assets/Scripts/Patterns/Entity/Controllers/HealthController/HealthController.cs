using System;
using UnityEngine;

[Serializable]
public class HealthController : IHealthController
{
    [SerializeField] private float _maxHealth = 100;
    public float MaxHealth => _maxHealth;

    private float _health;
    public float Health => _health;

    public event Action Died;
    public event Action Damaged;
    public event Action Healed;

    [field: SerializeField] public HealthAudioController AudioController { get; set; }

    public void Initialize()
    {
        AudioController.Initialize(this);
        _health = MaxHealth;
    }

    public void Damage(float damage)
    {
        if (damage <= 0 || _health == 0)
            return;

        _health = Math.Max(_health - damage, 0);
        Damaged?.Invoke();

        if (_health <= 0)
            Died?.Invoke();
    }

    public void Heal(float heal)
    {
        if (heal <= 0 || _health == MaxHealth)
            return;

        _health = Math.Min(_health + heal, MaxHealth);
        Healed?.Invoke();
    }

    public void SetMaxHealth(float maxHealth)
    {
        if (maxHealth <= 0)
        {
            Debug.LogWarning("Max health cannot be less than zero");
            return;
        }

        _maxHealth = maxHealth;
        _health = Math.Min(_maxHealth, _health);
    }
}
