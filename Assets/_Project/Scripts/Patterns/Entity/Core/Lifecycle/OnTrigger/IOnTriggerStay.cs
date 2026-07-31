using UnityEngine;

namespace EntityLib
{
    public interface IOnTriggerStay : IModuleEvent
    {
        void OnTriggerStay(Collider other);
    }
}
