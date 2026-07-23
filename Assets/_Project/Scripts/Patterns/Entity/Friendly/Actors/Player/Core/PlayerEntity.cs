using UnityEngine;

namespace Entity.Friendly.Player
{
    public class PlayerEntity : EntityBase
    {
        [Header("Modules")]
        [SerializeField] protected EntityRoleModule _roleModule;
        [SerializeField] protected EntityHealthModule _healthModule;
        [SerializeField] protected PlayerCurrencyModule _currencyModule;
        [Space(10)]
        [SerializeField] protected PlayerInputModule _inputModule;
        [SerializeField] protected PlayerMovementModule _moveModule;
        [SerializeField] protected FriendlyTargetModule _targetModule;
        [SerializeField] protected EntityWeaponModule _weaponsModule;
        [Space(10)]
        [SerializeField] protected PlayerAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;
        [SerializeField] protected PlayerShootLineModule _shootLineModule;
        [SerializeField] protected PlayerDeathModule _deathModule;
    }
}
