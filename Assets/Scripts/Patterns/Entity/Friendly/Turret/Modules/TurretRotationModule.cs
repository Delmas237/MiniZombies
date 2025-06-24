using System;
using UnityEngine;

[Serializable]
public class TurretRotationModule
{
    [SerializeField] private float _rotationSpeed = 13f;
    [SerializeField] private Transform _tower;

    private TurretAttackModule _attackModule;

    public void Initialize(TurretAttackModule attackModule)
    {
        _attackModule = attackModule;
    }

    public void Rotate()
    {
        if (_attackModule.IsFindingEnemy && _attackModule.ClosestEnemy != null)
            RotateToTarget(_attackModule.ClosestEnemy.Transform.position);
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
