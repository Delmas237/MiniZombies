using Entity;
using Entity.Hostile;
using UnityEngine;

namespace Player
{
    public interface IPlayerMovementModule : IEntityMovementModule
    {
        IHostile ClosestEnemy { get; }

        void Move(Vector2 direction);
        void RotateToDirection(Vector3 direction);
    }
}
