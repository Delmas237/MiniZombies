using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretAnimationModule
    {
        [SerializeField] private Animator _animator;

        private IEntityHealthModule _healthModule;
        private IEntityTargetModule _targetModule;
        private TurretAttackModule _attackModule;

        public void Initialize(IEntityHealthModule healthModule, IEntityTargetModule targetModule, TurretAttackModule attackModule)
        {
            _healthModule = healthModule;
            _targetModule = targetModule;
            _attackModule = attackModule;

            float fireLength = GetAnimationClipLength("Fire");
            _animator.SetFloat("FireSpeed", fireLength / attackModule.Cooldown);

            _attackModule.StartedInstalling += OnStartedInstalling;
            _healthModule.IsOver += OnHealthIsOver;
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

            if (_targetModule.Target != null)
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
            if (_attackModule != null)
                _attackModule.StartedInstalling -= OnStartedInstalling;
            if (_healthModule != null)
                _healthModule.IsOver -= OnHealthIsOver;
        }
    }
}
