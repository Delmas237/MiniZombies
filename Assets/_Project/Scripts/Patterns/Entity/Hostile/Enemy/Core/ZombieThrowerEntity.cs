using UnityEngine;

namespace Entity.Hostile
{
    public class ZombieThrowerEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyAvoidantMovementModule _moveModule;
        [SerializeField] protected ZombieThrowerAttackModule _attackModule;
        [SerializeField] protected EnemyDeathModule _deathModule;

        private void Shoot() => GetModule<IEnemyThrowerAttackModule>().Throw();
    }
}
