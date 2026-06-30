using System;
using UnityEngine;

namespace Entity.Hostile
{
    [Serializable]
    public class EnemyDeathModule : IEntityModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;

        private MonoBehaviour _monoBehaviour;
        private IEntityHealthModule _healthModule;
        private IEnemyMovementModule _movementModule;
        private IEnemyAttackModule _attackModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        public void Initialize(MonoBehaviour monoBehaviour, IEntityHealthModule healthModule, IEnemyMovementModule movementModule, IEnemyAttackModule attackModule)
        {
            _monoBehaviour = monoBehaviour;
            _healthModule = healthModule;
            _movementModule = movementModule;
            _attackModule = attackModule;

            _healthModule.IsOver += OnHealthIsOver;
        }
        private void OnHealthIsOver()
        {
            AddRigidbody();
            Disable();
        }

        private void Disable()
        {
            _movementModule.Agent.enabled = false;
            _attackModule.StopAttackImmediately();
            _monoBehaviour.enabled = false;
        }

        private void AddRigidbody()
        {
            if (!_monoBehaviour.GetComponent<Rigidbody>())
            {
                Rigidbody rb = _monoBehaviour.gameObject.AddComponent<Rigidbody>();

                rb.freezeRotation = true;
                rb.mass = 10f;
                rb.linearVelocity /= 2;
            }
        }

        public void Dispose()
        {
            _healthModule.IsOver -= OnHealthIsOver;
        }
    }
}
