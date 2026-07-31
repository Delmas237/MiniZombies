using UnityEngine.AI;

namespace EntityLib.Hostile
{
    public interface IEnemyMovementModule : IEntityMovementModule
    {
        NavMeshAgent Agent { get; }
        float Speed { get; set; }
    }
}
