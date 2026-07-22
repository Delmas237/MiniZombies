using UnityEngine;

namespace Entity.Hostile
{
    public abstract class ZombieEntity : EntityBase, IHostile
    {
        [Header("Base Modules")]
        [SerializeField] protected EntityHealthModule _healthModule;
        [SerializeField] protected EnemyTargetModule _targetModule;
        [SerializeField] protected EnemyRotationModule _rotationModule;
        [SerializeField] protected EnemyAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;
        [Space(10)]
        [SerializeField] protected EntityDelayedDisableModule _delayedDisableModule;
        [SerializeField] protected EntityDropAmmoOnDeathModule _dropAmmoAfterDeathModule;
    }
}
