using UnityEngine;

public interface IPlayerInputModule
{
    bool HasMoveInput { get; }
    bool IsTraking { get; }
}
