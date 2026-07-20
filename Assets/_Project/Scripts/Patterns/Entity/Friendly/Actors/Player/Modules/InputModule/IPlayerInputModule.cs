using UnityEngine;

namespace Entity.Friendly.Player
{
    public interface IPlayerInputModule : IModule
    {
        bool HasMoveInput { get; }
        bool IsTraking { get; }
        Vector2 MoveDirection { get; }
        Vector2 AttackDirection { get; }
    }
}
