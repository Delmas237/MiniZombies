using System.Collections;
using UnityEngine;
using System;

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static Coroutine StartRoutine(IEnumerator routine)
    {
        if (instance != null)
        {
            return instance.StartCoroutine(routine);
        }
        else
        {
            Debug.LogException(new NullReferenceException(nameof(CoroutineHelper) + "instance is null"));
            return null;
        }
    }
    public static void StopRoutine(Coroutine coroutine)
    {
        if (instance != null)
            instance.StopCoroutine(coroutine);
    }
}
