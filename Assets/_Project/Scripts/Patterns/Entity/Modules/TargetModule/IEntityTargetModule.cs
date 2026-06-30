namespace Entity
{
    public interface IEntityTargetModule : IModule
    {
        bool IsFindingTarget { get; set; }
        IEntity Target { get; set; }
    }
}
