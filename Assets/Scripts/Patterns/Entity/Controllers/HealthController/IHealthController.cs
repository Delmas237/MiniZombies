using System;

public interface IHealthController
{
    public float MaxHealth { get; set; }
    public float Health { get; }

    public event Action Decreased;
    public event Action Increased;
    public event Action IsOver;

    public void Decrease(float value);
    public void Increase(float value);
}
