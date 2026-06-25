namespace Entity.Friendly.Player
{
    public interface IPlayerInputModule
    {
        bool HasMoveInput { get; }
        bool IsTraking { get; }
    }
}
