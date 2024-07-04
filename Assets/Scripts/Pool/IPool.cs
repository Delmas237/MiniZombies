using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public interface IPool<out T> where T : Component
    {
        public T[] Prefabs { get; }
        public IReadOnlyList<T> Elements { get; }

        public event Action<T> Expanded;

        public T GetFreeElement();
    }
}
