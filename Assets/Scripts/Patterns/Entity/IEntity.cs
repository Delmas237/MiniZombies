using UnityEngine;

public interface IEntity
{
    public Transform Transform { get; }
    public IHealthModule HealthModule { get; }
}
