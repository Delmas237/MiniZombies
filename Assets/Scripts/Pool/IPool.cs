using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public interface IPool<out T> where T : Component
    {
        public T Prefab { get; }
        public IReadOnlyList<T> Elements { get; }

        public T GetFreeElement();
    }
}
