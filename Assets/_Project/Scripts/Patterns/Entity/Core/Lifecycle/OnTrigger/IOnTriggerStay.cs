using UnityEngine;

namespace Entity
{
    public interface IOnTriggerStay
    {
        void OnTriggerStay(Collider other);
    }
}
