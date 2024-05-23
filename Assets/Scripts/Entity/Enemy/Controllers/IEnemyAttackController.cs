namespace EnemyLib
{
    public interface IEnemyAttackController
    {
        public bool IsAttack { get; set; }
        public int Damage { get; }
    }
}
