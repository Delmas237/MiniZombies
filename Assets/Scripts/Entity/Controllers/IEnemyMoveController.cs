using UnityEngine.AI;

namespace EnemyLib
{
    public interface IEnemyMoveController : IMoveController
    {
        public NavMeshAgent Agent { get; }
        public IEntity Target { get; set; }
        public float DefaultSpeed { get; }
        public float Speed { get; set; }
    }
}
