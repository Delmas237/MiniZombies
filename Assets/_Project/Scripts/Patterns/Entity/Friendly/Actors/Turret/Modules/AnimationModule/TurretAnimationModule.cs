using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretAnimationModule : IModule, IOnStart, ILateUpdatable, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private Animator _animator;

        private IEntityHealthModule _healthModule;
        private IEntityTargetModule _targetModule;
        private ITurretAttackModule _attackModule;
        private ITurretInstallModule _installModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        [ModuleInject]
        private void Initialize(IEntityHealthModule healthModule, IEntityTargetModule targetModule, ITurretAttackModule attackModule, ITurretInstallModule installModule)
        {
            _healthModule = healthModule;
            _targetModule = targetModule;
            _attackModule = attackModule;
            _installModule = installModule;

            _installModule.InstallStarted += OnStartedInstalling;
            _healthModule.IsOver += OnHealthIsOver;
        }

        public void Start()
        {
            float fireLength = GetAnimationClipLength("Fire");
            _animator.SetFloat("FireSpeed", fireLength / _attackModule.Cooldown);
        }

        private float GetAnimationClipLength(string name)
        {
            List<AnimationClip> clips = _animator.runtimeAnimatorController.animationClips.ToList();
            AnimationClip clip = clips.Find(c => c.name == name);

            if (clip != null)
            {
                return clip.length;
            }
            else
            {
                Debug.LogWarning($"Animation '{name}' is not founded");
                return 1f;
            }
        }

        private void OnStartedInstalling()
        {
            if (!Enabled)
                return;

            _animator.SetTrigger("Install");
        }
        private void OnHealthIsOver()
        {
            if (!Enabled)
                return;

            _animator.SetTrigger("Death");
        }

        public virtual void LateUpdate()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            if (_healthModule.Health <= 0 || !_installModule.IsInstalled)
                return;

            if (_targetModule.Target != null)
            {
                _animator.SetTrigger("Fire");
            }
            else
            {
                _animator.SetTrigger("Idle");
            }
        }

        public void Dispose()
        {
            if (_installModule != null)
                _installModule.InstallStarted -= OnStartedInstalling;
            if (_healthModule != null)
                _healthModule.IsOver -= OnHealthIsOver;
        }
    }
}
