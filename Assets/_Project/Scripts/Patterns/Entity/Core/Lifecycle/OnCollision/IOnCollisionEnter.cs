using UnityEngine;

namespace Entity
{
    public interface IOnCollisionEnter
    {
        void OnCollisionEnter(Collision collision);
    }
}
