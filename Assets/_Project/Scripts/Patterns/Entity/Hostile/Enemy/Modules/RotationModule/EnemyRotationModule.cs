using System;
using UnityEngine;

namespace EntityLib.Hostile
{
    [Serializable]
    public class EnemyRotationModule : IModule, IUpdatable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private float _anglePreset = 30f;

        protected Transform _transform;
        protected IEntityTargetModule _targetModule;
        protected IEnemyAttackModule _attackModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        [ModuleInject]
        private void Initialize(Transform transform, IEntityTargetModule targetModule, IEnemyAttackModule attackModule)
        {
            _transform = transform;

            _targetModule = targetModule;
            _attackModule = attackModule;
        }

        public virtual void Update()
        {
            Rotate();
        }
        protected virtual void Rotate()
        {
            if (_targetModule.Target != null && _targetModule.Target.HealthModule.Health > 0 && _attackModule.IsAttack)
            {
                Vector3 targetPos = _targetModule.Target.Transform.position - _transform.position;
                targetPos = new Vector3(targetPos.x, 0, targetPos.z);

                _transform.rotation = Quaternion.LookRotation(targetPos);
                _transform.eulerAngles += Vector3.up * _anglePreset;
            }
        }
    }
}
