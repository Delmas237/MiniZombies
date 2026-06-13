using UnityEngine.AI;

namespace EnemyLib
{
    public interface IEnemyMovementModule : IMovementModule
    {
        NavMeshAgent Agent { get; }
        IEntity Target { get; set; }
        float Speed { get; set; }
    }
}
