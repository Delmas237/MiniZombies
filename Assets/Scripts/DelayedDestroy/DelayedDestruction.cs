using System.Collections;
using UnityEngine;

public class DelayedDestruction : MonoBehaviour
{
    [SerializeField] protected float _delay = 2f;

    protected virtual void Start()
    {
        Destroy(gameObject, _delay);
    }

    private IEnumerator SetActiveFalse(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
 
    protected void DelayedSetActiveFalse() => StartCoroutine(SetActiveFalse(_delay));
}
