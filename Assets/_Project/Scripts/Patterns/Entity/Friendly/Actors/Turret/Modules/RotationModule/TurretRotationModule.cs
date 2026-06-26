using System;
using UnityEngine;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretRotationModule : IEntityModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private float _rotationSpeed = 13f;
        [SerializeField] private Transform _tower;

        private IEntityTargetModule _targetModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        public void Initialize(IEntityTargetModule targetModule)
        {
            _targetModule = targetModule;
        }

        public void Rotate()
        {
            if (_targetModule.Target != null)
                RotateToTarget(_targetModule.Target.Transform.position);
        }
        private void RotateToTarget(Vector3 target)
        {
            Vector3 direction = target - _tower.position;
            direction.y = 0;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float currentAngle = _tower.eulerAngles.y;

            float smoothedAngle = Mathf.LerpAngle(currentAngle, targetAngle, _rotationSpeed * Time.deltaTime);
            _tower.rotation = Quaternion.Euler(_tower.eulerAngles.x, smoothedAngle, _tower.eulerAngles.z);
        }
    }
}
