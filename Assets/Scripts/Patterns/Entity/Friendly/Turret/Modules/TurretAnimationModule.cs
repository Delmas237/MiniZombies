using System;
using UnityEngine;

[Serializable]
public class TurretAnimationModule
{
    [SerializeField] private Animator _animator;

    private IHealthModule _healthModule;
    private TurretAttackModule _attackModule;

    public void Initialize(IHealthModule healthModule, TurretAttackModule attackModule)
    {
        _healthModule = healthModule;
        _attackModule = attackModule;

        _attackModule.StartedInstalling += OnStartedInstalling;
        _healthModule.IsOver += OnHealthIsOver;
    }
    private void OnStartedInstalling()
    {
        _animator.SetTrigger("Install");
    }
    private void OnHealthIsOver()
    {
        _animator.SetTrigger("Death");
    }

    public void UpdateState()
    {
        if (_healthModule.Health <= 0 || !_attackModule.IsInstalled)
            return;

        if (_attackModule.IsFindingEnemy && _attackModule.ClosestEnemy != null)
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
        _attackModule.StartedInstalling -= OnStartedInstalling;
        _healthModule.IsOver -= OnHealthIsOver;
    }
}
