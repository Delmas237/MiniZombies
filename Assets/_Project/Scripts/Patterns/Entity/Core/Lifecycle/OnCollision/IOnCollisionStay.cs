using UnityEngine;

namespace EntityLib
{
    public interface IOnCollisionStay : IModuleEvent
    {
        void OnCollisionStay(Collision collision);
    }
}
