namespace EnemyLib
{
    public interface IEnemyAttackController
    {
        public bool IsAttack { get; set; }
        public float AttackSpeed { get; set; }
        public int Damage { get; }
    }
}
