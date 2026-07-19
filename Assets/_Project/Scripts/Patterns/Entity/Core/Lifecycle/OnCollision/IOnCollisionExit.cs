using UnityEngine;

namespace Entity
{
    public interface IOnCollisionExit : IModuleEvent
    {
        void OnCollisionExit(Collision collision);
    }
}
