using Entity;
using UnityEngine;

namespace Player
{
    public interface IPlayerMovementModule : IEntityMovementModule
    {
        void Move(Vector2 direction);
        void RotateToDirection(Vector3 direction);
    }
}
