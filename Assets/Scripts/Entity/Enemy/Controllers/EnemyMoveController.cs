using PlayerLib;
using System;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

namespace EnemyLib
{
    [Serializable]
    public class EnemyMoveController
    {
        private NavMeshAgent agent;
        public NavMeshAgent Agent => agent;

        public PlayerContainer Target { get; set; }
        [field: SerializeField] public float DefaultSpeed { get; set; } = 3.7f;
        public float Speed { get; set; }

        public void Initialize(NavMeshAgent _agent)
        {
            agent = _agent;
        }
        public void OnEnable()
        {
            Agent.speed = Speed;
        }

        public void Move()
        {
            if (Target && Target.HealthController.Health > 0)
            {
                if (Agent.enabled)
                    Agent.SetDestination(Target.transform.position);
            }
            else
            {
                Agent.enabled = false;
            }
        }
    }
}
