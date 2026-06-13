using EnemyLib;
using Weapons;

public interface IHostile : IEntity
{
    IEnemyMovementModule MovementModule { get; }
    DropAmmoAfterDeathModule DropAmmoAfterDeathModule { get; }
}
