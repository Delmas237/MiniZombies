using EnemyLib;
using Weapons;

public interface IEnemy : IEntity
{
    IEnemyMoveModule MoveModule { get; }
    DropAmmoAfterDeathModule DropAmmoAfterDeathModule { get; }
}
