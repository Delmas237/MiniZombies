using UnityEngine;

namespace Entity.Hostile
{
    public class ZombieThrowerEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyAvoidantMovementModule _moveModule;
        [SerializeField] protected ZombieThrowerAttackModule _attackModule;
        [SerializeField] protected EnemyDeathModule _deathModule;

        public IEnemyThrowerAttackModule ThrowerAttackModule => _attackModule;
        public override IEnemyMovementModule MovementModule => _moveModule;
        public override IEnemyAttackModule AttackModule => _attackModule;

        private void Shoot() => ThrowerAttackModule.Throw();
    }
}
