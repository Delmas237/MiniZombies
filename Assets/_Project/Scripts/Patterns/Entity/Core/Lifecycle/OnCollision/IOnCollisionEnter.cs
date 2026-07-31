using UnityEngine;

namespace EntityLib
{
    public interface IOnCollisionEnter : IModuleEvent
    {
        void OnCollisionEnter(Collision collision);
    }
}
