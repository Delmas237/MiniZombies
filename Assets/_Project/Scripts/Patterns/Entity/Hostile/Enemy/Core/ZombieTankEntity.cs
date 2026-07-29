using UnityEngine;

namespace Entity.Hostile
{
    public class ZombieTankEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyMovementModule _moveModule;
        [SerializeField] protected EnemyMeleeAttackModule _attackModule;
        [SerializeField] protected EnemyDeathModule _deathModule;
    }
}
