using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class TurretAnimationModule
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Motion _installMotion;

    private bool _isInstalled;
    private IHealthModule _healthModule;
    private TurretRotationModule _rotationModule;

    public void Initialize(IHealthModule healthModule, TurretRotationModule rotationModule)
    {
        _healthModule = healthModule;
        _rotationModule = rotationModule;

        _healthModule.IsOver += OnHealthIsOver;
    }
    private IEnumerator WaitInstallMotion()
    {
        yield return new WaitForSeconds(_installMotion.averageDuration);
        _isInstalled = true;
    }

    public void InstallAnim()
    {
        _isInstalled = false;
        _animator.SetTrigger("Install");
        CoroutineHelper.StartRoutine(WaitInstallMotion());
    }

    private void OnHealthIsOver()
    {
        _animator.SetTrigger("Death");
    }

    public void UpdateState()
    {
        if (_healthModule.Health <= 0 || !_isInstalled)
            return;

        if (_rotationModule.ClosestEnemy != null)
        {
            _animator.SetTrigger("Fire");
        }
        else
        {
            _animator.SetTrigger("Idle");
        }
    }

    public void OnDestroy()
    {
        _healthModule.IsOver -= OnHealthIsOver;
    }
}
