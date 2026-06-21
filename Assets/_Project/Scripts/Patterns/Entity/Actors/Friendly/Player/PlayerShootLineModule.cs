using Player;
using UnityEngine;

public class PlayerShootLineModule
{
    [SerializeField] private Transform _shootLineRoot;

    private IPlayerWeaponModule _weaponModule;

    public const float START_DISTANCE = 0.848f;

    public void Initialize(IPlayerWeaponModule weaponModule)
    {
        _weaponModule = weaponModule;
    }

    public void UpdateShootLineScale()
    {
        float distance = _weaponModule.CurrentGun.Distance + START_DISTANCE;
        _shootLineRoot.localScale = new Vector3(1, 1, distance);
    }

    public void UpdateShootLine(Vector2 direction)
    {
        bool attackJoystickMoving = direction != Vector2.zero;
        if (attackJoystickMoving)
            _shootLineRoot.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));

        _shootLineRoot.gameObject.SetActive(attackJoystickMoving);
    }
}
