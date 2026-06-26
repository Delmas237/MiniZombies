using System;
using UnityEngine;

namespace Entity.Friendly.Player
{
    [Serializable]
    public class PlayerShootLineModule : IEntityModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private Transform _shootLineRoot;

        private IPlayerWeaponModule _weaponModule;

        public const float START_DISTANCE = 0.848f;

        public bool Enabled { get => _enabled; set => _enabled = value; }

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
            bool isZero = direction == Vector2.zero;
            if (!isZero)
                _shootLineRoot.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));

            _shootLineRoot.gameObject.SetActive(!isZero);
        }
    }
}
