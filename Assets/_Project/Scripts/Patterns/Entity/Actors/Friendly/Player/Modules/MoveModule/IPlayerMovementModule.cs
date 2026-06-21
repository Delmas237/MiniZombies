using Entity;
using Entity.Hostile;

namespace Player
{
    public interface IPlayerMovementModule : IEntityMovementModule
    {
        bool IsMoving { get; }
        bool IsTraking { get; }
        IHostile ClosestEnemy { get; }

        void RotateToDirection(Vector2 direction);
    }
}
