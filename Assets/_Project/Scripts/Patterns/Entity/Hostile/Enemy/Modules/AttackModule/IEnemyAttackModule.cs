namespace Entity.Hostile
{
    public interface IEnemyAttackModule : IModule
    {
        bool IsAttack { get; }
        float Speed { get; set; }
        float BaseSpeed { get; }
        int Damage { get; }

        void Attack();
        void StopAttackImmediately();
    }
}
