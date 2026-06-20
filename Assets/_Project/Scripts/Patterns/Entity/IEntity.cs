using UnityEngine;

namespace Entity
{
    public interface IEntity
    {
        Transform Transform { get; }
        IEntityHealthModule HealthModule { get; }
    }
}
