using EventBusLib;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity
{
    [Serializable]
    public class EntityAudioModule : IModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] protected AudioSource _damageSound;
        [SerializeField] protected AudioSource _healSound;
        [SerializeField] protected AudioSource _deathSound;
        [Space(10)]
        [SerializeField] protected float _deathPitchRandomRange;

        private IEntityHealthModule _healthModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        
        public virtual void Initialize(IEntityHealthModule healthModule)
        {
            _healthModule = healthModule;

            _healthModule.IsOver += OnHealhIsOver;
            _healthModule.Increased += OnHealhIncreased;
            _healthModule.Decreased += OnHealthDecreased;

            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent exitEvent = null)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            if (_healthModule == null)
                return;

            _healthModule.IsOver -= OnHealhIsOver;
            _healthModule.Increased -= OnHealhIncreased;
            _healthModule.Decreased -= OnHealthDecreased;
        }

        private void OnHealhIsOver()
        {
            if (!_enabled)
                return;

            if (_deathSound == null)
                return;

            _deathSound.pitch = Random.Range(_deathSound.pitch - _deathPitchRandomRange, _deathSound.pitch + _deathPitchRandomRange);
            _deathSound.Play();
        }
        private void OnHealhIncreased()
        {
            if (_healSound == null)
                return;

            _healSound.Play();
        }
        private void OnHealthDecreased()
        {
            if (_damageSound == null)
                return;
            
            _damageSound.Play();
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}
