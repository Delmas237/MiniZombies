using UnityEngine.AI;

namespace EnemyLib
{
    public interface IEnemyMoveController : IMoveController
    {
        public NavMeshAgent Agent { get; }
        public IPlayer Target { get; set; }
        public float DefaultSpeed { get; set; }
        public float Speed { get; set; }
    }
}
