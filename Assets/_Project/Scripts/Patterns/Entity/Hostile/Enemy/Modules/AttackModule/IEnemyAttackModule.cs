namespace Entity.Hostile
{
    public interface IEnemyAttackModule : IModule
    {
        bool IsAttack { get; }
        float Speed { get; set; }
        float DefaultSpeed { get; }
        int Damage { get; }

        void StopAttackImmediately();
    }
}
