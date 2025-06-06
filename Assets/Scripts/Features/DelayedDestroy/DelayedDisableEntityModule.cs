using EventBusLib;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DelayedDisableEntityModule : IOptionalModule
{
    [SerializeField] private bool _enabled = true;
    [Space(5)]
    [SerializeField] private float _delay = 3f;

    private GameObject _gameObject;
    private IEntity _entity;

    public bool Enabled => _enabled;

    public void Initialize(GameObject gameObject, IEntity entity)
    {
        if (_enabled)
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
