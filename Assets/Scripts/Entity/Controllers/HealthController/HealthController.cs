using System;
using UnityEngine;

[Serializable]
public class HealthController : IHealthController
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100;
    private float _health;
    public float Health
    {
        get { return _health; }
        set
        {
            value = Mathf.Clamp(value, 0, MaxHealth);

            if (value == 0 && _health > 0)
            {
                _health = value;
                Damaged?.Invoke();
                Died?.Invoke();
            }
            else if (value < _health)
            {
                _health = value;
                Damaged?.Invoke();
            }
            else if (value > _health)
            {
                _health = value;
                Healed?.Invoke();
            }
        }
    }

    public event Action Died;
    public event Action Damaged;
    public event Action Healed;

    [field: SerializeField] public HealthAudioController AudioController { get; set; }

    public void Initialize()
    {
        AudioController.Initialize(this);
        _health = MaxHealth;
    }
}
