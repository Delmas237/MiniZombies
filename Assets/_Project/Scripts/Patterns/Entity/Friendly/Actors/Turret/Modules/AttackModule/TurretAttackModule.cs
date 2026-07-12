using System;
using UnityEngine;
using Weapons;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretAttackModule : ITurretAttackModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private Transform _visibilityZone;
        [SerializeField] private float _defaultVisibilityZoneScale = 0.21f;

        private IEntityTargetModule _targetModule;
        private IEntityWeaponModule _weaponModule;
        private ITurretInstallModule _installModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public float Cooldown => _weaponModule.CurrentGun.Cooldown;

        public void Initialize(IEntityTargetModule targetModule, IEntityWeaponModule weaponModule, ITurretInstallModule installModule)
        {
            _targetModule = targetModule;
            _weaponModule = weaponModule;
            _installModule = installModule;

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

        public virtual void Attack()
        {
            if (!_enabled)
                return;

            if (!_installModule.IsInstalled || _targetModule.Target == null)
                return;

            _weaponModule.PullTrigger();
        }

        public void Dispose()
        {
            if (_weaponModule != null)
                _weaponModule.GunChanged -= UpdateVisibilityZone;
        }
    }
}