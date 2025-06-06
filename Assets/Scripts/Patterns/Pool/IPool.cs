using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public interface IPool<out T> : IInstanceProvider<T> where T : Component
    {
        public event Action<T> Expanded;

        public T[] Prefabs { get; }
        public IReadOnlyList<T> Elements { get; }
    }
}
