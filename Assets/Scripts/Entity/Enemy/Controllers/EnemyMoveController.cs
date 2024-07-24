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

        protected Transform _transform;
        private IEnemyAttackController _attackController;

        public IEntity Target { get; set; }

        [SerializeField] protected float _defaultSpeed = 3.7f;
        public float DefaultSpeed => _defaultSpeed;
        public float Speed { get; set; }

        public void Initialize(NavMeshAgent agent, Transform transform, IEnemyAttackController attackController)
        {
            _agent = agent;
            _transform = transform;
            _attackController = attackController;
        }

        public virtual void UpdateData()
        {
            Agent.speed = Speed;
        }

        public virtual void Move()
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

        public virtual void Rotate()
        {
            if (Target != null && Target.HealthController.Health > 0 && _attackController.IsAttack)
            {
                Vector3 targetPos = Target.Transform.position - _transform.position;
                targetPos = new Vector3(targetPos.x, 0, targetPos.z);

                _transform.rotation = Quaternion.LookRotation(targetPos);
                _transform.eulerAngles += Vector3.up * 30;
            }
        }
    }
}
