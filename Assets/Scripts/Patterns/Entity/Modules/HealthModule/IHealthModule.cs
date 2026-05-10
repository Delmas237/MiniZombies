using System;

public interface IHealthModule
{
    event Action Decreased;
    event Action Increased;
    event Action IsOver;

    float MaxHealth { get; set; }
    float Health { get; }

    void Decrease(float value);
    void Increase(float value);
}
