using UnityEngine;

namespace Entity
{
    public interface IOnAwake : IModuleEvent
    {
        void Awake();
    }
}
