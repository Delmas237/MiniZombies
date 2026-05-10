using UnityEngine.AI;

namespace EnemyLib
{
    public interface IEnemyMoveModule : IMoveModule
    {
        NavMeshAgent Agent { get; }
        IEntity Target { get; set; }
        float Speed { get; set; }
    }
}
