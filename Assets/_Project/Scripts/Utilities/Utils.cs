using System;
using UnityEngine;

public static class Utils
{
    public static void Timer(ref float time, Action finished)
    {
        if (time > 0)
            time -= Time.deltaTime;
        else
            finished.Invoke();
    }

    public static string Watch(in int time)
    {
        return $"{time / 60}:{time % 60:D2}";
    }
}
