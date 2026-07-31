using System;
using UnityEngine;

namespace EntityLib.Friendly.Player
{
    [Serializable]
    public class PlayerDeathModule : IModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;

        private IEntityHealthModule _healthModule;
        private IPlayerMovementModule _movementModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }


        [ModuleInject]
        private void Initialize(IEntityHealthModule healthModule, IPlayerMovementModule movementModule)
        {
            _healthModule = healthModule;
            _movementModule = movementModule;

            _healthModule.IsOver += OnHealthIsOver;
        }
        private void OnHealthIsOver()
        {
            if (!Enabled)
                return;

            _movementModule.Rigidbody.linearVelocity /= 2;
        }

        public void Dispose()
        {
            if (_healthModule != null)
                _healthModule.IsOver -= OnHealthIsOver;
        }
    }
}
