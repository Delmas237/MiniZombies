using Weapons;

namespace Entity.Hostile
{
    public interface IHostile : IEntity
    {
        IEnemyMovementModule MovementModule { get; }
        EntityDropAmmoOnDeathModule DropAmmoAfterDeathModule { get; }
    }
}
