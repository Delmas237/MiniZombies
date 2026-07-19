using UnityEngine;

namespace Entity.Hostile
{
    public class ZombieShooterEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyMovementModule _moveModule;
        [SerializeField] protected ZombieShooterAttackModule _attackModule;
        [SerializeField] protected EntityWeaponModule _weaponsModule;
        [SerializeField] protected EnemyDeathModule _deathModule;

        public override IEnemyMovementModule MovementModule => _moveModule;
        public override IEnemyAttackModule AttackModule => _attackModule;
        public IEntityWeaponModule WeaponModule => _weaponsModule;

        private void Shoot() => _weaponsModule.PullTrigger();
    }
}
