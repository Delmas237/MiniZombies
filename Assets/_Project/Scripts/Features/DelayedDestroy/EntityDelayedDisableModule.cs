using EventBusLib;
using System;
using System.Collections;
using UnityEngine;

namespace Entity
{
    [Serializable]
    public class EntityDelayedDisableModule : IModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private float _delay = 3f;

        private IEntityHealthModule _healthModule;
        private GameObject _gameObject;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        public void Initialize(GameObject gameObject, IEntityHealthModule healthModule)
        {
            _gameObject = gameObject;
            _healthModule = healthModule;

            _healthModule.IsOver += DelayedSetActiveFalse;
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent exitEvent)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            _healthModule.IsOver -= DelayedSetActiveFalse;
        }

        private IEnumerator SetActiveFalse(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (!_enabled)
                yield break;

            if (_gameObject != null)
                _gameObject.SetActive(false);
        }

        protected void DelayedSetActiveFalse() => CoroutineHelper.StartRoutine(SetActiveFalse(_delay));
    }
}
