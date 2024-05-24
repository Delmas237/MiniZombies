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

        private Transform _transform;
        private IEnemyAttackController _attackController;

        public IPlayer Target { get; set; }

        [field: SerializeField] public float DefaultSpeed { get; set; } = 3.7f;
        public float Speed { get; set; }
        [Space(10)]
        [SerializeField] private bool _rotateTowardsTarget;

        public void Initialize(NavMeshAgent agent, Transform transform, IEnemyAttackController attackController)
        {
            _agent = agent;
            _transform = transform;
            _attackController = attackController;
            Agent.speed = Speed;
        }

        public void OnEnable()
        {
            if (Agent != null)
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

        public void Rotation()
        {
            if (Target != null && Target.HealthController.Health > 0 && _rotateTowardsTarget)
            {
                Vector3 targetPos = Target.Transform.position - _transform.position;
                targetPos = new Vector3(targetPos.x, 0, targetPos.z);

                _transform.rotation = Quaternion.LookRotation(targetPos);

                if (_attackController.IsAttack)
                    _transform.eulerAngles += Vector3.up * 30;
            }
        }
    }
}
