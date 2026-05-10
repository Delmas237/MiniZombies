using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public interface IPool<out T> : IInstanceProvider<T> where T : Component
    {
        event Action<T> Expanded;

        T[] Prefabs { get; }
        IReadOnlyList<T> Elements { get; }
    }
}
