using UnityEngine;

namespace Entity.Hostile
{
    public class ZombieTankEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyMovementModule _moveModule;
        [SerializeField] protected EnemyTankAttackModule _attackModule;
        [SerializeField] protected EnemyDeathModule _deathModule;

        private void DealDamage() => _attackModule.DealDamage();
    }
}
