namespace Entity
{
    public interface IEntityTargetModule : IEntityModule
    {
        bool IsFindingTarget { get; set; }
        IEntity Target { get; set; }
    }
}
