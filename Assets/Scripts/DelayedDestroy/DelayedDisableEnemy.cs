using EnemyLib;

public class DelayedDisableEnemy : DelayedDestruction
{
    protected override void Start()
    {
        GetComponent<EnemyContainer>().HealthController.Died += DelayedSetActiveFalse;
    }
}
