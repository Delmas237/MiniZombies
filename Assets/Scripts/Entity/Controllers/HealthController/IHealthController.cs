using System;

public interface IHealthController
{
    public float MaxHealth { get; set; }
    public float Health { get; set; }

    public event Action Died;
    public event Action Damaged;
    public event Action Healed;
}
