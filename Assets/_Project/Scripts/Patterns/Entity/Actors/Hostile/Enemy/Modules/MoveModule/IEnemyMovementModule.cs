using UnityEngine.AI;

namespace Entity.Hostile
{
    public interface IEnemyMovementModule : IEntityMovementModule
    {
        NavMeshAgent Agent { get; }
        IEntity Target { get; set; }
        float Speed { get; set; }

        void Move();
    }
}
