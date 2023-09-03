using EnemyLib;

public class DelayedDisableEnemy : DelayedDestruction
{
    private HealthController healthController;

    protected override void Start()
    {
        healthController = GetComponent<Enemy>().HealthController;
        healthController.Died += DelayedSetActiveFalse;
    }
    protected void OnDestroy()
    {
        healthController.Died -= DelayedSetActiveFalse;
    }
}
