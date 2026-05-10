using UnityEngine;

public interface IEntity
{
    Transform Transform { get; }
    IHealthModule HealthModule { get; }
}
