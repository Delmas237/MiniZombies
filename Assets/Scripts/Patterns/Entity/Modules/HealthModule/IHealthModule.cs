using System;

public interface IHealthModule
{
    public event Action Decreased;
    public event Action Increased;
    public event Action IsOver;

    public float MaxHealth { get; set; }
    public float Health { get; }

    public void Decrease(float value);
    public void Increase(float value);
}
