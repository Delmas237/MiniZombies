using System;
using System.Collections;
using UnityEngine;
using Weapons;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretAttackModule : IModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private Transform _visibilityZone;
        [SerializeField] private float _defaultVisibilityZoneScale = 0.21f;
        [Space(10)]
        [SerializeField] private Motion _installMotion;

        private bool _isInstalled;
        private Coroutine _installCoroutine;

        private IEntityTargetModule _targetModule;
        private IEntityWeaponModule _weaponModule;

        public event Action InstallStarted;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (!_enabled)
                        StopInstallImmediately();
                }
            }
        }

        public bool IsInstalled => _isInstalled;
        public float Cooldown => _weaponModule.CurrentGun.Cooldown;

        public void Initialize(IEntityTargetModule targetModule, IEntityWeaponModule weaponModule)
        {
            _targetModule = targetModule;
            _weaponModule = weaponModule;
            UpdateVisibilityZone();

            _weaponModule.GunChanged += UpdateVisibilityZone;
        }

        private void UpdateVisibilityZone(Gun gun) => UpdateVisibilityZone();
        
        private void UpdateVisibilityZone()
        {
            if (!_enabled)
                return;

            if (_visibilityZone == null || _weaponModule.CurrentGun == null)
                return;

            _visibilityZone.localScale = _defaultVisibilityZoneScale * _weaponModule.CurrentGun.Distance * Vector3.one;
        }

        public void Install()
        {
            if (!_enabled)
                return;

            if (_installCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_installCoroutine);
                _installCoroutine = null;
            }

            _installCoroutine = CoroutineHelper.StartRoutine(InstallCoroutine());
        }

        private IEnumerator InstallCoroutine()
        {
            _isInstalled = false;
            InstallStarted?.Invoke();

            float duration = _installMotion.averageDuration;
            yield return new WaitForSeconds(duration);

            if (!_enabled)
                yield break;

            _isInstalled = true;
            _installCoroutine = null;
        }

        public void StopInstallImmediately()
        {
            _isInstalled = false;

            if (_installCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_installCoroutine);
                _installCoroutine = null;
            }
        }

        public virtual void Attack()
        {
            if (!_enabled)
                return;

            if (!_isInstalled || _targetModule.Target == null)
                return;

            _weaponModule.PullTrigger();
        }

        public void Dispose()
        {
            StopInstallImmediately();

            if (_weaponModule != null)
                _weaponModule.GunChanged -= UpdateVisibilityZone;
        }
    }
}