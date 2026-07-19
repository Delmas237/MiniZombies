using UnityEngine;

namespace Entity
{
    public interface IOnTriggerExit : IModuleEvent
    {
        void OnTriggerExit(Collider other);
    }
}
