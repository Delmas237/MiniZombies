using UnityEngine;

namespace EntityLib
{
    public interface IOnTriggerExit : IModuleEvent
    {
        void OnTriggerExit(Collider other);
    }
}
