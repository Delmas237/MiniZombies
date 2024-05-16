using EnemyLib;

public class DelayedDisableEnemy : DelayedDestruction
{
    protected override void Start()
    {
        GetComponent<IEntity>().HealthController.Died += DelayedSetActiveFalse;
    }
}
