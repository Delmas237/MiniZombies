namespace EnemyLib
{
    public interface IEnemyAttackModule
    {
        bool IsAttack { get; set; }
        float Speed { get; set; }
        float DefaultSpeed { get; }
        int Damage { get; }
    }
}
