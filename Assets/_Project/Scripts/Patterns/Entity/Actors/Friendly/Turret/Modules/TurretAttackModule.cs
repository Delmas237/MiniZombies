using System;
using System.Collections;
using UnityEngine;
using Weapons;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretAttackModule
    {
        [SerializeField] private Transform _visibilityZone;
        [SerializeField] private float _defaultVisibilityZoneScale = 0.21f;
        [Space(10)]
        [SerializeField] private Motion _installMotion;

        private bool _isInstalled;
        private IEntityTargetModule _targetModule;
        private IEntityWeaponModule _weaponModule;

        public event Action StartedInstalling;

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
            _visibilityZone.localScale = _defaultVisibilityZoneScale * _weaponModule.CurrentGun.Distance * Vector3.one;
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
            if (_isInstalled && _targetModule.Target != null)
                _weaponModule.PullTrigger();
        }

        public void OnDestroy()
        {
            if (_weaponModule != null)
                _weaponModule.GunChanged -= UpdateVisibilityZone;
        }
    }
}
