using UnityEngine.AI;

namespace EnemyLib
{
    public interface IEnemyMoveModule : IMoveModule
    {
        public NavMeshAgent Agent { get; }
        public IEntity Target { get; set; }
        public float Speed { get; set; }
    }
}
