using UnityEngine;

namespace Entity
{
    public interface IOnCollisionStay : IModuleEvent
    {
        void OnCollisionStay(Collision collision);
    }
}
