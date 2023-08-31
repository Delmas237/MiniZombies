using System.Collections;
using UnityEngine;

public class DelayedDestruction : MonoBehaviour
{
    [SerializeField] protected float delay = 2f;
    [SerializeField] protected bool ifDisabledDestroy;

    protected void DelayedDestroy() => Destroy(gameObject, delay);
    protected void DelayedSetActiveFalse() => StartCoroutine(SetActiveFalse(delay));

    protected virtual void Start()
    {
        Destroy(gameObject, delay);
    }

    protected void OnDisable()
    {
        if (ifDisabledDestroy)
            Destroy(gameObject);
    }

    private IEnumerator SetActiveFalse(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
