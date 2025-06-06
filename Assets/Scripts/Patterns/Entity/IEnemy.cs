using EnemyLib;
using Weapons;

public interface IEnemy : IEntity
{
    public IEnemyMoveModule MoveModule { get; }
    public DropAmmoAfterDeathModule DropAmmoAfterDeathModule { get; }
}
