using System;
using UnityEngine;
using Weapons;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretVisibilityZoneModule : IModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private Transform _visibilityZone;
        [SerializeField] private float _defaultVisibilityZoneScale = 0.21f;

        private IEntityWeaponModule _weaponModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        public void Initialize(IEntityWeaponModule weaponModule)
        {
            _weaponModule = weaponModule;

            UpdateVisibilityZone();
            _weaponModule.GunChanged += UpdateVisibilityZone;
        }

        private void UpdateVisibilityZone(Gun gun) => UpdateVisibilityZone();

        private void UpdateVisibilityZone()
        {
            if (!Enabled)
                return;

            if (_visibilityZone == null || _weaponModule.CurrentGun == null)
                return;

            _visibilityZone.localScale = _defaultVisibilityZoneScale * _weaponModule.CurrentGun.Distance * Vector3.one;
        }

        public void Dispose()
        {
            if (_weaponModule != null)
                _weaponModule.GunChanged -= UpdateVisibilityZone;
        }
    }
}
