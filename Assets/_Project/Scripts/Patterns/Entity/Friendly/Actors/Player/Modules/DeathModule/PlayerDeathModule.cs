using System;
using UnityEngine;

namespace Entity.Friendly.Player
{
    [Serializable]
    public class PlayerDeathModule : IEntityModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;

        private IEntityHealthModule _healthModule;
        private IPlayerMovementModule _movementModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        public void Initialize(IEntityHealthModule healthModule, IPlayerMovementModule movementModule)
        {
            _healthModule = healthModule;
            _movementModule = movementModule;

            _healthModule.IsOver += OnHealthIsOver;
        }
        private void OnHealthIsOver()
        {
            _movementModule.Rigidbody.linearVelocity /= 2;
        }

        public void Dispose()
        {
            _healthModule.IsOver -= OnHealthIsOver;
        }
    }
}
