using System;
using UnityEngine;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretAttackModule : ITurretAttackModule, IUpdatable
    {
        [SerializeField] private bool _enabled = true;

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
        }

        public virtual void Update()
        {
            Attack();
        }

        private void Attack()
        {
            if (!_installModule.IsInstalled || _targetModule.Target == null)
                return;

            _weaponModule.PullTrigger();
        }
    }
}