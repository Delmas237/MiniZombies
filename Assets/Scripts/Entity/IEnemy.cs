using EnemyLib;
using Weapons;

public interface IEnemy : IEntity
{
    public EnemyAnimationController AnimationController { get; set; }
    public DropAmmoAfterDeathModule DropAmmoAfterDeathModule { get; set; }
}
