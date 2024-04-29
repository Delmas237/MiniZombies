using PlayerLib;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    [Serializable]
    public class EnemyMoveController
    {
        public NavMeshAgent Agent { get; set; }
        public PlayerContainer Target { get; set; }
        [field: SerializeField] public float Speed { get; set; } = 3.5f;

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
