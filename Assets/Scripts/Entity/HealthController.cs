using PlayerLib;
using System;
using UnityEngine;

[Serializable]
public class HealthController
{
    [field: SerializeField] public float MaxHealth { get; private set; } = 100;
    private float health;
    public float Health
    {
        get { return health; }
        set
        {
            if (value <= 0 && health > 0)
                Death();
            if (value > MaxHealth)
                value = MaxHealth;

            if (value < health)
                Damaged?.Invoke();
            if (value > health)
                Healed?.Invoke();

            health = value;
        }
    }

    public event Action Died;
    public event Action Damaged;
    public event Action Healed;

    [field: SerializeField] public HealthAudioController AudioController { get; set; }

    public void Initialize()
    {
        AudioController.Initialize(this);
        health = MaxHealth;
    }
    private void Death()
    {
        //enabled = false;
        Died?.Invoke();
    }
}
