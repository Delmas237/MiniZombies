using UnityEngine;

public interface IEntity
{
    Transform Transform { get; }
    IEntityHealthModule HealthModule { get; }
}
