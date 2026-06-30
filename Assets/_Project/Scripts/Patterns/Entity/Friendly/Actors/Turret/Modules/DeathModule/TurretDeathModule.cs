using System;
using UnityEngine;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretDeathModule : IModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;

        private IEntityHealthModule _healthModule;
        private IModule _attackModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        public void Initialize(IEntityHealthModule healthModule, IModule attackModule)
        {
            _healthModule = healthModule;
            _attackModule = attackModule;

            _healthModule.IsOver += OnHealthIsOver;
        }
        private void OnHealthIsOver()
        {
            _attackModule.Enabled = false;
        }

        public void Dispose()
        {
            if (_healthModule == null)
                return;

            _healthModule.IsOver -= OnHealthIsOver;
        }
    }
}
