using System.Collections;
using UnityEngine;

public class DelayedDestruction : MonoBehaviour
{
    [SerializeField] protected float _delay = 2f;
    [SerializeField] protected bool _destroyWhenDisabled;

    protected void DelayedDestroy() => Destroy(gameObject, _delay);
    protected void DelayedSetActiveFalse() => StartCoroutine(SetActiveFalse(_delay));

    protected virtual void Start()
    {
        Destroy(gameObject, _delay);
    }

    protected void OnDisable()
    {
        if (_destroyWhenDisabled)
            Destroy(gameObject);
    }

    private IEnumerator SetActiveFalse(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
