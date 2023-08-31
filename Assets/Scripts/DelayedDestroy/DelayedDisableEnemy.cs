using EnemyLib;

public class DelayedDisableEnemy : DelayedDestruction
{
    protected override void Start()
    {
        GetComponent<Enemy>().HealthController.Died += DelayedSetActiveFalse;
    }
}
