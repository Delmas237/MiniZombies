using UnityEngine;

namespace Entity
{
    public interface IOnTriggerEnter
    {
        void OnTriggerEnter(Collider other);
    }
}
