using System.Collections;
using UnityEngine;
using System;

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static Coroutine StartRoutine(IEnumerator routine)
    {
        if (_instance != null)
        {
            return _instance.StartCoroutine(routine);
        }
        else
        {
            Debug.LogException(new NullReferenceException(nameof(CoroutineHelper) + "instance is null"));
            return null;
        }
    }
    public static void StopRoutine(Coroutine coroutine)
    {
        if (_instance != null)
            _instance.StopCoroutine(coroutine);
    }
}
