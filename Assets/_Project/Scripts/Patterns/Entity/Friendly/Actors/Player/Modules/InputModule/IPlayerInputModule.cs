namespace Entity.Friendly.Player
{
    public interface IPlayerInputModule : IModule
    {
        bool HasMoveInput { get; }
        bool IsTraking { get; }
    }
}
