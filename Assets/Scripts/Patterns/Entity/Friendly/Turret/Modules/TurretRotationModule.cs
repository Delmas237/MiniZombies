using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TurretRotationModule
{
    [SerializeField] private float _rotationSpeed = 13f;
    [SerializeField] private float _timeToUpdateClosestEnemy = 0.35f;
    [Space(10)]
    [SerializeField] private Transform _tower;

    private IWeaponsModule _weaponsModule;

    private IEnemy _closestEnemy;
    private Coroutine _closestEnemyCoroutine;

    public IEnemy ClosestEnemy => _closestEnemy;

    public void Initialize(IWeaponsModule weaponsModule)
    {
        _weaponsModule = weaponsModule;
        _closestEnemyCoroutine = CoroutineHelper.StartRoutine(UpdateClosestEnemy());
    }

    public void Rotate()
    {
        if (_closestEnemy != null)
        {
            _weaponsModule.PullTrigger();
            RotateToClosestEnemy(_closestEnemy.Transform.position);
        }
    }
    private void RotateToClosestEnemy(Vector3 closestEnemy)
    {
        Vector3 direction = closestEnemy - _tower.position;
        direction.y = 0;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float currentAngle = _tower.eulerAngles.y;

        float smoothedAngle = Mathf.LerpAngle(currentAngle, targetAngle, _rotationSpeed * Time.deltaTime);
        _tower.rotation = Quaternion.Euler(_tower.eulerAngles.x, smoothedAngle, _tower.eulerAngles.z);
    }

    private IEnumerator UpdateClosestEnemy()
    {
        while (true)
        {
            IReadOnlyList<IEnemy> closestEnemies = Spawner<IEnemy>.ObjectsOnScene;

            bool enemyInRange = false;
            IEnemy closestEnemy = null;
            if (closestEnemies.Count > 0)
            {
                Vector3 position = _weaponsModule.CurrentGun.transform.position;
                List<Transform> enemiesTransform = closestEnemies.Select(e => e.Transform).ToList();
                Transform transform = ComponentSearcher<Transform>.Closest(position, enemiesTransform);
                closestEnemy = closestEnemies[enemiesTransform.IndexOf(transform)];

                float distanceToEnemy = Vector3.Distance(position, closestEnemy.Transform.position);
                enemyInRange = distanceToEnemy <= _weaponsModule.CurrentGun.Distance;
            }
            _closestEnemy = enemyInRange ? closestEnemy : null;

            yield return new WaitForSeconds(_timeToUpdateClosestEnemy);
        }
    }

    public void OnDestroy()
    {
        CoroutineHelper.StopRoutine(_closestEnemyCoroutine);
    }
}
