using System;
using UnityEngine;
using UnityEngine.AI;

namespace EntityLib.Hostile
{
    [Serializable]
    public class EnemyMovementModule : IEnemyMovementModule, IUpdatable
    {
        [SerializeField] protected bool _enabled = true;
        [Space(10)]
        [SerializeField] protected float _defaultSpeed = 3.7f;

        protected float _speed;
        protected Transform _transform;
        protected NavMeshAgent _agent;
        protected IEntityTargetModule _targetModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public float Speed
        {
            get => _speed;
            set
            {
                if (Agent == null)
                {
                    Debug.LogWarning($"{nameof(Agent)} is not initialized");
                    return;
                }
                _speed = value;
                Agent.speed = _speed;
            }
        }

        public float DefaultSpeed => _defaultSpeed;
        public NavMeshAgent Agent => _agent;

        [ModuleInject]
        protected void Initialize(Transform transform, NavMeshAgent agent, IEntityTargetModule targetModule)
        {
            _transform = transform;
            _agent = agent;

            _targetModule = targetModule;
        }

        public virtual void Update()
        {
            Move();
        }

        protected virtual void Move()
        {
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
    }
}
