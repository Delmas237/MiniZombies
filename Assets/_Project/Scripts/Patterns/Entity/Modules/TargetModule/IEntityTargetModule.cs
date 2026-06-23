namespace Entity
{
    public interface IEntityTargetModule
    {
        bool IsFindingTarget { get; set; }
        IEntity Target { get; set; }
    }
}
