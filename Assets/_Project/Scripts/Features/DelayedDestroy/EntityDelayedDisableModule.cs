using EventBusLib;
using System;
using System.Collections;
using UnityEngine;

namespace EntityLib
{
    [Serializable]
    public class EntityDelayedDisableModule : IModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private float _delay = 3f;

        private IEntityHealthModule _healthModule;
        private Transform _transform;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        [ModuleInject]
        private void Initialize(Transform transform, IEntityHealthModule healthModule)
        {
            _transform = transform;
            _healthModule = healthModule;

            _healthModule.IsOver += DelayedSetActiveFalse;
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent exitEvent = null)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            if (_healthModule != null)
                _healthModule.IsOver -= DelayedSetActiveFalse;
        }

        private IEnumerator SetActiveFalse(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (!Enabled)
                yield break;

            if (_transform != null)
                _transform.gameObject.SetActive(false);
        }

        protected void DelayedSetActiveFalse() => CoroutineHelper.StartRoutine(SetActiveFalse(_delay));

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}
