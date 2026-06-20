using EventBusLib;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity
{
    [Serializable]
    public class EntityAudioModule
    {
        [SerializeField] protected AudioSource _damageSound;
        [SerializeField] protected AudioSource _healSound;
        [SerializeField] protected AudioSource _deathSound;
        [Space(10)]
        [SerializeField] protected float _deathPitchRandomRange;

        private IEntityHealthModule _healthModule;

        public virtual void Initialize(IEntityHealthModule healthModule)
        {
            _healthModule = healthModule;

            _healthModule.IsOver += OnHealhIsOver;
            _healthModule.Increased += OnHealhIncreased;
            _healthModule.Decreased += OnHealthDecreased;

            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent exitEvent)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            _healthModule.IsOver -= OnHealhIsOver;
            _healthModule.Increased -= OnHealhIncreased;
            _healthModule.Decreased -= OnHealthDecreased;
        }

        private void OnHealhIsOver()
        {
            if (_deathSound != null)
            {
                _deathSound.pitch = Random.Range(_deathSound.pitch - _deathPitchRandomRange, _deathSound.pitch + _deathPitchRandomRange);
                _deathSound.Play();
            }
        }
        private void OnHealhIncreased()
        {
            if (_healSound != null)
                _healSound.Play();
        }
        private void OnHealthDecreased()
        {
            if (_damageSound != null)
                _damageSound.Play();
        }
    }
}
