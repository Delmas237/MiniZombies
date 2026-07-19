using UnityEngine;

namespace Entity
{
    public interface IOnCollisionEnter : IModuleEvent
    {
        void OnCollisionEnter(Collision collision);
    }
}
