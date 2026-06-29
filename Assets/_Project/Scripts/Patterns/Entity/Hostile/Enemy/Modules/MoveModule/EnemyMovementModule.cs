using System;
using UnityEngine;
using UnityEngine.AI;

namespace Entity.Hostile
{
    [Serializable]
    public class EnemyMovementModule : IEnemyMovementModule
    {
        [SerializeField] protected bool _enabled = true;
        [Space(10)]
        [SerializeField] protected float _defaultSpeed = 3.7f;
        
        protected Transform _transform;
        protected NavMeshAgent _agent;
        protected IEntityTargetModule _targetModule;
        protected IEnemyAttackModule _attackModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public float Speed { get; set; }

        public float DefaultSpeed => _defaultSpeed;
        public NavMeshAgent Agent => _agent;

        public void Initialize(Transform transform, NavMeshAgent agent, IEntityTargetModule targetModule, IEnemyAttackModule attackModule)
        {
            _transform = transform;
            _agent = agent;

            _targetModule = targetModule;
            _attackModule = attackModule;
            
            Speed = DefaultSpeed;
        }

        public virtual void UpdateData()
        {
            Agent.speed = Speed;
        }

        public virtual void Move()
        {
            if (!_enabled)
                return;

            if (_targetModule.Target != null && _targetModule.Target.HealthModule.Health > 0)
            {
                if (Agent.enabled)
                    Agent.SetDestination(_targetModule.Target.Transform.position);
            }
            else
            {
                Agent.enabled = false;
            }
        }

        public virtual void Rotate()
        {
            if (!_enabled)
                return;

            if (_targetModule.Target != null && _targetModule.Target.HealthModule.Health > 0 && _attackModule.IsAttack)
            {
                Vector3 targetPos = _targetModule.Target.Transform.position - _transform.position;
                targetPos = new Vector3(targetPos.x, 0, targetPos.z);

                _transform.rotation = Quaternion.LookRotation(targetPos);
                _transform.eulerAngles += Vector3.up * 30;
            }
        }
    }
}
