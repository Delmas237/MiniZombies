using UnityEngine;

namespace ObjectPool
{
    public interface IPool<out T> where T : Component
    {
        public T Prefab { get; }

        public T GetFreeElement();
    }
}
