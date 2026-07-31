using UnityEngine;

namespace EntityLib
{
    public interface IOnTriggerEnter : IModuleEvent
    {
        void OnTriggerEnter(Collider other);
    }
}
