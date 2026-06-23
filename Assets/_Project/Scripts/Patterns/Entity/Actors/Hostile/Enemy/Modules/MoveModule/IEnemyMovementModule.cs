using UnityEngine.AI;

namespace Entity.Hostile
{
    public interface IEnemyMovementModule : IEntityMovementModule
    {
        NavMeshAgent Agent { get; }
        float Speed { get; set; }

        void Move();
    }
}
