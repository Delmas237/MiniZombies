using UnityEngine;

namespace Entity
{
    public interface IOnTriggerEnter : IModuleEvent
    {
        void OnTriggerEnter(Collider other);
    }
}
