using UnityEngine;

namespace Entity.Friendly.Turret
{
    public class TurretEntity : EntityBase, IFriendly
    {
        [Header("Modules")]
        [SerializeField] protected EntityHealthModule _healthModule;
        [Space(10)]
        [SerializeField] protected FriendlyTargetModule _targetModule;
        [SerializeField] protected EntityWeaponModule _weaponsModule;
        [SerializeField] protected TurretAttackModule _attackModule;
        [SerializeField] protected TurretRotationModule _rotationModule;
        [SerializeField] protected TurretInstallModule _installModule;
        [SerializeField] protected TurretVisibilityZoneModule _visibilityZoneModule;
        [Space(10)]
        [SerializeField] protected TurretAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;
        [SerializeField] protected TurretDeathModule _deathModule;
    }
}
