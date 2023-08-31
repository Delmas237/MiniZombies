using UnityEngine;

namespace ObjectPool
{
    public interface IPool<T> where T : Component
    {
        public T GetFreeElement();
    }
}
