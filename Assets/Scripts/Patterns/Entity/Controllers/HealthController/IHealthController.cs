using System;

public interface IHealthController
{
    public float MaxHealth { get; }
    public float Health { get; }

    public event Action Died;
    public event Action Damaged;
    public event Action Healed;

    public void Damage(float damage);
    public void Heal(float heal);

    public void SetMaxHealth(float maxHealth);
}
