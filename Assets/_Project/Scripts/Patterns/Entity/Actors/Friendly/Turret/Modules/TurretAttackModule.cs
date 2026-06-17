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
    private IWeaponModule _weaponModule;

    private IHostile _closestEnemy;
    private bool _isFindingEnemy = true;
    private Coroutine _closestEnemyCoroutine;

    public event Action StartedInstalling;

    public bool IsInstalled => _isInstalled;
    public bool IsFindingEnemy => _isFindingEnemy;
    public IHostile ClosestEnemy => _closestEnemy;
    public float Cooldown => _weaponModule.CurrentGun.Cooldown;

    public void Initialize(IWeaponModule weaponModule)
    {
        _weaponModule = weaponModule;
        _closestEnemyCoroutine = CoroutineHelper.StartRoutine(UpdateClosestEnemy());
        UpdateVisibilityZone();

        _weaponModule.GunChanged += UpdateVisibilityZone;
        EventBus.Subscribe<GameOverEvent>(OnGameOver);
    }
    private void UpdateVisibilityZone(Gun gun) => UpdateVisibilityZone();
    private void UpdateVisibilityZone()
    {
        _visibilityZone.localScale = _defaultVisibilityZoneScale * _weaponModule.CurrentGun.Distance * Vector3.one;
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
            _weaponModule.PullTrigger();
        }
    }

    private IEnumerator UpdateClosestEnemy()
    {
        while (true)
        {
            IReadOnlyList<IHostile> closestEnemies = Spawner<IHostile>.ObjectsOnScene;

            bool enemyInRange = false;
            IHostile closestEnemy = null;
            if (closestEnemies.Count > 0)
            {
                Vector3 position = _weaponModule.CurrentGun.transform.position;
                List<Transform> enemiesTransform = closestEnemies.Select(e => e.Transform).ToList();
                Transform transform = ComponentSearcher<Transform>.Closest(position, enemiesTransform);
                closestEnemy = closestEnemies[enemiesTransform.IndexOf(transform)];

                float distanceToEnemy = Vector3.Distance(position, closestEnemy.Transform.position);
                enemyInRange = distanceToEnemy <= _weaponModule.CurrentGun.Distance;
            }
            _closestEnemy = enemyInRange ? closestEnemy : null;

            yield return new WaitForSeconds(_timeToUpdateClosestEnemy);
        }
    }

    public void OnDestroy()
    {
        if (_closestEnemyCoroutine != null)
            CoroutineHelper.StopRoutine(_closestEnemyCoroutine);

        if (_weaponModule != null)
            _weaponModule.GunChanged -= UpdateVisibilityZone;
        EventBus.Unsubscribe<GameOverEvent>(OnGameOver);
    }
}
