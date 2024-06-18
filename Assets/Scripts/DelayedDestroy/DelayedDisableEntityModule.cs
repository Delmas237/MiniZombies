using EventBusLib;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DelayedDisableEntityModule : IModule
{
    [field: SerializeField] public bool Enabled { get; set; } = true;
    [Space(5)]
    [SerializeField] protected float _delay = 3f;

    private GameObject _gameObject;
    private IEntity _entity;

    public void Initialize(GameObject gameObject, IEntity entity)
    {
        if (Enabled)
        {
            _gameObject = gameObject;
            _entity = entity;

            entity.HealthController.Died += DelayedSetActiveFalse;
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
    }
    private void Unsubscribe(GameExitEvent exitEvent)
    {
        EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
        _entity.HealthController.Died -= DelayedSetActiveFalse;
    }

    private IEnumerator SetActiveFalse(float delay)
    {
        yield return new WaitForSeconds(delay);
        _gameObject.SetActive(false);
    }

    protected void DelayedSetActiveFalse() => CoroutineHelper.StartRoutine(SetActiveFalse(_delay));
}
