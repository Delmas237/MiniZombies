using UnityEngine;

public interface IEntity
{
    public Transform Transform { get; }
    public IHealthController HealthController { get; }
}
