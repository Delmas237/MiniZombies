using UnityEngine;

namespace EntityLib
{
    public interface IOnAwake : IModuleEvent
    {
        void Awake();
    }
}
