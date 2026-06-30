using UnityEngine;

namespace Entity.Friendly.Player
{
    public interface IPlayerMovementModule : IEntityMovementModule
    {
        Rigidbody Rigidbody { get; }

        void Move(Vector2 direction);
        void RotateToDirection(Vector3 direction);
    }
}
