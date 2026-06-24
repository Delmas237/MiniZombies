using UnityEngine;

namespace Player
{
    public interface IPlayerInputModule
    {
        bool HasMoveInput { get; }
        bool IsTraking { get; }
    }
}
