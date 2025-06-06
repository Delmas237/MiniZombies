using System;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyLib
{
    [Serializable]
    public class EnemyMoveModule : IEnemyMoveModule
    {
        [SerializeField] protected float _defaultSpeed = 3.7f;
        
        protected Transform _transform;
        private NavMeshAgent _agent;
        private IEnemyAttackModule _attackModule;

        public IEntity Target { get; set; }
        public float Speed { get; set; }

        public float DefaultSpeed => _defaultSpeed;
        public NavMeshAgent Agent => _agent;

        public void Initialize(NavMeshAgent agent, Transform transform, IEnemyAttackModule attackModule)
        {
            _agent = agent;
            _transform = transform;
            _attackModule = attackModule;
            Speed = DefaultSpeed;
        }

        public virtual void UpdateData()
        {
            Agent.speed = Speed;
        }

        public virtual void Move()
        {
            if (Target != null && Target.HealthModule.Health > 0)
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
            if (Target != null && Target.HealthModule.Health > 0 && _attackModule.IsAttack)
            {
                Vector3 targetPos = Target.Transform.position - _transform.position;
                targetPos = new Vector3(targetPos.x, 0, targetPos.z);

                _transform.rotation = Quaternion.LookRotation(targetPos);
                _transform.eulerAngles += Vector3.up * 30;
            }
        }
    }
}
