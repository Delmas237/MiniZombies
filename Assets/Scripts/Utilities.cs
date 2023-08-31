using System;
using UnityEngine;

public static class Utilities
{
    public static void Timer(ref float time)
    {
        if (time > 0)
            time -= Time.deltaTime;
    }
    public static void Timer(ref float time, Action finished)
    {
        if (time > 0)
            time -= Time.deltaTime;
        else
            finished.Invoke();
    }

    public static string Watch(in int time)
    {
        string line;

        if (time % 60 > 9)
            line =
                $"{time / 60}:" +
                $"{time % 60}";
        else
            line =
                $"{time / 60}:" +
                $"0{time % 60}";

        return line;
    }
}
