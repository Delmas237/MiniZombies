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

    public void Initialize(GameObject gameObject, IEntity entity)
    {
        _gameObject = gameObject;
        entity.HealthController.Died += DelayedSetActiveFalse;
    }
    private IEnumerator SetActiveFalse(float delay)
    {
        yield return new WaitForSeconds(delay);
        _gameObject.SetActive(false);
    }

    protected void DelayedSetActiveFalse() => CoroutineHelper.StartRoutine(SetActiveFalse(_delay));
}
