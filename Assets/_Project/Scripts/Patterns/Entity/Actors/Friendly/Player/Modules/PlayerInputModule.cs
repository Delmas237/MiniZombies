using Entity;
using Player;
using UnityEngine;
using Weapons;

public class PlayerInputModule
{
    [SerializeField] private PlayerMobileInput _mobileInput;

    private Transform _transform;
    private IEntityHealthModule _healthModule;
    private IPlayerMovementModule _movementModule;
    private IPlayerWeaponModule _weaponModule;
    private PlayerShootLineModule _shootLineModule;

    public void Initialize(Transform transform, IEntityHealthModule healthModule, IPlayerMovementModule movementModule, IPlayerWeaponModule weaponModule, PlayerShootLineModule shootLineModule)
    {
        _transform = transform;
        _healthModule = healthModule;
        _movementModule = movementModule;
        _weaponModule = weaponModule;
        _shootLineModule = shootLineModule;

        _healthModule.IsOver += Unsubscribe;
        _weaponModule.GunChanged += UpdateShootLineScale;

        _mobileInput.AttackJoystick.Drag += UpdateShootLine;
        _mobileInput.AttackJoystick.OnUp += _weaponModule.PullTrigger;
        _mobileInput.AttackJoystick.OnClamped += _weaponModule.PullAutoTrigger;

        _mobileInput.MoveJoystick.Drag += Rotate;
    }
    private void UpdateShootLineScale(Gun gun)
    {
        _shootLineModule.UpdateShootLineScale();
    }
    private void UpdateShootLine()
    {
        _shootLineModule.UpdateShootLine(_mobileInput.AttackJoystick.Direction);
    }


    private void Rotate()
    {
        if (_mobileInput.AttackJoystick.Direction != Vector2.zero)
        {
            _movementModule.RotateToDirection(_mobileInput.AttackJoystick.Direction, _rotationSpeed * Time.deltaTime);
        }
        else if (_mobileInput.AttackJoystick.UnPressedOrInDeadZoneTime > 0.15f)
        {
            if (_closestEnemy != null)
            {
                Vector3 direction = _movementModule.ClosestEnemy.Transform.position;
                direction -= _transform.position;
                direction = new Vector3(direction.x, 0, direction.z);
                _movementModule.RotateToDirection(direction);
            }
            else if (_mobileInput.MoveJoystick.Direction != Vector2.zero && _mobileInput.AttackJoystick.UnPressedOrInDeadZoneTime > 0.05f)
            {
                _movementModule.RotateToDirection(_mobileInput.MoveJoystick, _rotationSpeed * Time.deltaTime);
            }
        }
    }


    public void OnDestroy()
    {
        Unsubscribe();
    }
    private void Unsubscribe()
    {
        _healthModule.IsOver -= Unsubscribe;
        _weaponModule.GunChanged -= UpdateShootLineScale;

        _mobileInput.AttackJoystick.Drag -= UpdateShootLine;
        _mobileInput.AttackJoystick.OnUp -= _weaponModule.PullTrigger;
        _mobileInput.AttackJoystick.OnClamped -= _weaponModule.PullAutoTrigger;

        _mobileInput.MoveJoystick.Drag -= Rotate;
    }
}
