using EventBusLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Weapons;

[Serializable]
public class TurretAttackModule
{
    [SerializeField] private Transform _visibilityZone;
    [SerializeField] private float _defaultVisibilityZoneScale = 0.21f;
    [Space(10)]
    [SerializeField] private float _timeToUpdateClosestEnemy = 0.35f;
    [Space(10)]
    [SerializeField] private Motion _installMotion;

    private bool _isInstalled;
    private IWeaponsModule _weaponsModule;

    private IEnemy _closestEnemy;
    private bool _isFindingEnemy = true;
    private Coroutine _closestEnemyCoroutine;

    public event Action StartedInstalling;

    public bool IsInstalled => _isInstalled;
    public bool IsFindingEnemy => _isFindingEnemy;
    public IEnemy ClosestEnemy => _closestEnemy;
    public float Cooldown => _weaponsModule.CurrentGun.Cooldown;

    public void Initialize(IWeaponsModule weaponsModule)
    {
        _weaponsModule = weaponsModule;
        _closestEnemyCoroutine = CoroutineHelper.StartRoutine(UpdateClosestEnemy());
        UpdateVisibilityZone();

        _weaponsModule.GunChanged += UpdateVisibilityZone;
        EventBus.Subscribe<GameOverEvent>(OnGameOver);
    }
    private void UpdateVisibilityZone(Gun gun) => UpdateVisibilityZone();
    private void UpdateVisibilityZone()
    {
        _visibilityZone.localScale = _defaultVisibilityZoneScale * _weaponsModule.CurrentGun.Distance * Vector3.one;
    }

    private void OnGameOver(GameOverEvent gameOverEvent)
    {
        _isFindingEnemy = false;
        CoroutineHelper.StopRoutine(_closestEnemyCoroutine);
        _closestEnemyCoroutine = null;
    }

    public void Install()
    {
        CoroutineHelper.StartRoutine(WaitInstallMotion());
    }
    private IEnumerator WaitInstallMotion()
    {
        _isInstalled = false;
        StartedInstalling?.Invoke();
        yield return new WaitForSeconds(_installMotion.averageDuration);
        _isInstalled = true;
    }

    public void Attack()
    {
        if (_isInstalled && _isFindingEnemy && _closestEnemy != null)
        {
            _weaponsModule.PullTrigger();
        }
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
        if (_closestEnemyCoroutine != null)
            CoroutineHelper.StopRoutine(_closestEnemyCoroutine);

        _weaponsModule.GunChanged -= UpdateVisibilityZone;
        EventBus.Unsubscribe<GameOverEvent>(OnGameOver);
    }
}
