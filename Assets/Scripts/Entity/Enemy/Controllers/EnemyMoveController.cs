using PlayerLib;
using System;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class EnemyMoveController
    {
        [field: SerializeField] public float Speed { get; set; } = 3.5f;
        public PlayerContainer Target { get; set; }

        private EnemyContainer enemy;

        public void Initialize(EnemyContainer _enemy)
        {
            enemy = _enemy;
        }

        public void Move()
        {
            if (Target && Target.HealthController.Health > 0)
            {
                if (enemy.Agent.enabled)
                    enemy.Agent.SetDestination(Target.transform.position);
            }
            else
            {
                enemy.Agent.enabled = false;
            }
        }
    }
}
