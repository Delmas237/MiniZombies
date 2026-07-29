using UnityEngine;

namespace Entity.Hostile
{
    public class ZombieThrowerEntity : ZombieEntity
    {
        [Header("Modules")]
        [SerializeField] protected EnemyAvoidantMovementModule _moveModule;
        [SerializeField] protected EnemyThrowerAttackModule _attackModule;
        [SerializeField] protected EnemyDeathModule _deathModule;
    }
}
