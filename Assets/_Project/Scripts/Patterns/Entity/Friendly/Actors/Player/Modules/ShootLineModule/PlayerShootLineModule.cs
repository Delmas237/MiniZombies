using System;
using UnityEngine;
using Weapons;

namespace EntityLib.Friendly.Player
{
    [Serializable]
    public class PlayerShootLineModule : IModule, IUpdatable, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private Transform _shootLineRoot;

        private IPlayerInputModule _inputModule;
        private IEntityWeaponModule _weaponModule;

        public const float START_DISTANCE = 0.848f;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        [ModuleInject]
        private void Initialize(IPlayerInputModule inputModule, IEntityWeaponModule weaponModule)
        {
            _inputModule = inputModule;
            _weaponModule = weaponModule;

            _weaponModule.GunChanged += UpdateShootLineScale;
        }
        private void UpdateShootLineScale(Gun gun)
        {
            if (!Enabled)
                return;

            UpdateShootLineScale();
        }

        public void UpdateShootLineScale()
        {
            if (!Enabled)
                return;

            if (_weaponModule == null)
                return;

            float distance = _weaponModule.CurrentGun.Distance + START_DISTANCE;
            _shootLineRoot.localScale = new Vector3(1, 1, distance);
        }

        public void Update()
        {
            UpdateShootLine(_inputModule.AttackDirection);
        }

        public void UpdateShootLine(Vector2 direction)
        {
            if (!Enabled)
                return;

            bool isZero = direction == Vector2.zero;
            if (!isZero)
                _shootLineRoot.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));

            _shootLineRoot.gameObject.SetActive(!isZero);
        }

        public void Dispose()
        {
            if (_weaponModule != null)
                _weaponModule.GunChanged -= UpdateShootLineScale;
        }
    }
}
