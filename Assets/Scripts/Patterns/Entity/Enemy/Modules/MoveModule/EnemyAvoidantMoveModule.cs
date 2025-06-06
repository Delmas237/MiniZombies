using System;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    [Serializable]
    public class EnemyAvoidantMoveModule : EnemyMoveModule
    {
        [Space(10)]
        [SerializeField] private float _minAvoidanceDistance = 10;
        [SerializeField] private float _maxAvoidanceDistance = 20;
        [Space(10)]
        [SerializeField] private float _fleeDistance = 5;

        public override void Move()
        {
            float distanceToTarget = Vector3.Distance(Target.Transform.position, _transform.position);

            if (Target != null && Target.HealthModule.Health > 0)
            {
                if (Agent.enabled)
                    Avoid(distanceToTarget);
            }
            else
            {
                Agent.enabled = false;
            }
        }

        private void Avoid(float distanceToTarget)
        {
            bool avoid = false;
            float targetDistance = 0;
            Vector3 startPosition = Vector3.zero;
            Vector3 fleeDirection = _transform.position - Target.Transform.position;

            if (distanceToTarget < _minAvoidanceDistance)
            {
                avoid = true;
                targetDistance = _fleeDistance;
                startPosition = _transform.position;
            }
            else if (distanceToTarget > _maxAvoidanceDistance)
            {
                avoid = true;
                targetDistance = Math.Max(_minAvoidanceDistance, _maxAvoidanceDistance / 2);
                startPosition = Target.Transform.position;
            }

            if (avoid)
            {
                Vector3 fleePosition = startPosition + fleeDirection.normalized * targetDistance;
                if (NavMesh.SamplePosition(fleePosition, out NavMeshHit hit, targetDistance, NavMesh.AllAreas))
                    Agent.SetDestination(hit.position);
            }
        }
    }
}
