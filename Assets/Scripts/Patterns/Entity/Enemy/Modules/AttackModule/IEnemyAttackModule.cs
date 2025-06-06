namespace EnemyLib
{
    public interface IEnemyAttackModule
    {
        public bool IsAttack { get; set; }
        public float Speed { get; set; }
        public float DefaultSpeed { get; }
        public int Damage { get; }
    }
}
