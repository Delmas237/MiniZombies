using UnityEngine;

namespace Entity.Hostile
{
    public class ZombieShooterEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyMovementModule _moveModule;
        [SerializeField] protected EnemyShooterAttackModule _attackModule;
        [SerializeField] protected EntityWeaponModule _weaponsModule;
        [SerializeField] protected EnemyDeathModule _deathModule;
    }
}
