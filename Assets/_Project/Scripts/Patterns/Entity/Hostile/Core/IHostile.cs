using Weapons;

namespace Entity.Hostile
{
    public interface IHostile : IEntity
    {
        IEnemyMovementModule MovementModule { get; }
        IEntityTargetModule TargetModule { get; }
        IEntityDropAmmoOnDeathModule DropAmmoAfterDeathModule { get; }
    }
}
