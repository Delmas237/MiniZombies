namespace EnemyLib
{
    public interface IEnemyAttackController
    {
        public bool IsAttack { get; set; }
        public float DefaultSpeed { get; }
        public float Speed { get; set; }
        public int Damage { get; }
    }
}
