using UnityEngine;

namespace EntityLib
{
    public interface IOnCollisionExit : IModuleEvent
    {
        void OnCollisionExit(Collision collision);
    }
}
