using System;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    [Serializable]
    public class EnemyMoveController : IEnemyMoveController
    {
        private NavMeshAgent _agent;
        public NavMeshAgent Agent => _agent;

        public IPlayer Target { get; set; }

        [field: SerializeField] public float DefaultSpeed { get; set; } = 3.7f;
        public float Speed { get; set; }

        public void Initialize(NavMeshAgent agent)
        {
            _agent = agent;
        }

        public void OnEnable()
        {
            Agent.speed = Speed;
        }

        public void Move()
        {
            if (Target != null && Target.HealthController.Health > 0)
            {
                if (Agent.enabled)
                    Agent.SetDestination(Target.Transform.position);
            }
            else
            {
                Agent.enabled = false;
            }
        }
    }
}
