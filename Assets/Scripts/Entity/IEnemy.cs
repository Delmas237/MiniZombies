using EnemyLib;
using Weapons;

public interface IEnemy : IEntity
{
    public IEnemyMoveController MoveController { get; }
    public DropAmmoAfterDeathModule DropAmmoAfterDeathModule { get; }
}
