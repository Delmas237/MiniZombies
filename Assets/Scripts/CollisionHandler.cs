using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public event Action<Collision> OnCollisionEnterEvent;
    public event Action<Collision> OnCollisionStayEvent;
    public event Action<Collision> OnCollisionExitEvent;

    private bool inCollision;
    public bool InCollision => inCollision;

    private void OnCollisionEnter(Collision collision)
    {
        inCollision = true;
        OnCollisionEnterEvent?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        inCollision = false;
        OnCollisionExitEvent?.Invoke(collision);
    }
}
