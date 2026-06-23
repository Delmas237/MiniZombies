using EventBusLib;
using System;
using System.Collections;
using UnityEngine;

namespace Entity
{
    [Serializable]
    public class EntityDelayedDisableModule : IEntityOptionalModule
    {
        [SerializeField] private bool _isEnabled = true;
        [Space(5)]
        [SerializeField] private float _delay = 3f;

        private GameObject _gameObject;
        private IEntity _entity;

        public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }

        public void Initialize(GameObject gameObject, IEntity entity)
        {
            if (_isEnabled)
            {
                _gameObject = gameObject;
                _entity = entity;

                entity.HealthModule.IsOver += DelayedSetActiveFalse;
                EventBus.Subscribe<GameExitEvent>(Unsubscribe);
            }
        }
        private void Unsubscribe(GameExitEvent exitEvent)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            _entity.HealthModule.IsOver -= DelayedSetActiveFalse;
        }

        private IEnumerator SetActiveFalse(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (_gameObject != null)
                _gameObject.SetActive(false);
        }

        protected void DelayedSetActiveFalse() => CoroutineHelper.StartRoutine(SetActiveFalse(_delay));
    }
}
