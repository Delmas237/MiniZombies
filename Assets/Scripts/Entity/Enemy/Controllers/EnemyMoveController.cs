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
        [field: SerializeField] public float DefaultSpeed { get; set; } = 3.7f;
        public float Speed { get; set; }

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
