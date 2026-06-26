namespace Entity.Hostile
{
    public interface IEnemyAttackModule : IEntityModule
    {
        bool IsAttack { get; set; }
        float Speed { get; set; }
        float DefaultSpeed { get; }
        int Damage { get; }
    }
}
