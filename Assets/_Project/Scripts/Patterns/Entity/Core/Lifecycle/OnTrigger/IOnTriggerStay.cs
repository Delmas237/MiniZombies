using UnityEngine;

namespace Entity
{
    public interface IOnTriggerStay : IModuleEvent
    {
        void OnTriggerStay(Collider other);
    }
}
