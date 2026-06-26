namespace Entity.Friendly.Player
{
    public interface IPlayerInputModule : IEntityModule
    {
        bool HasMoveInput { get; }
        bool IsTraking { get; }
    }
}
