using UnityEngine;
using Weapons;

namespace EnemyLib
{
    public abstract class ZombieContainer : MonoBehaviour, IEnemy
    {
        [Header("Base Controllers")]
        [SerializeField] protected HealthController _healthController;
        public IHealthController HealthController => _healthController;

        [SerializeField] protected EnemyMoveController _moveController;
        public IEnemyMoveController MoveController => _moveController;
        public abstract IEnemyAttackController AttackController { get; }

        [SerializeField] protected EnemyAnimationController _animationController;

        [field: Header("Base Modules")]
        [SerializeField] protected DelayedDisableEntityModule _delayedDisableModule;
        public DelayedDisableEntityModule DelayedDisableModule => _delayedDisableModule;

        [SerializeField] protected DropAmmoAfterDeathModule _dropAmmoAfterDeathModule;
        public DropAmmoAfterDeathModule DropAmmoAfterDeathModule => _dropAmmoAfterDeathModule;

        public Transform Transform => transform;

        protected virtual void Awake()
        {
            HealthController.Died += OnDeath;
        }
        protected virtual void OnDestroy()
        {
            HealthController.Died -= OnDeath;
        }

        protected virtual void OnDeath()
        {
            if (GetComponent<Rigidbody>() == false)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();

                rb.freezeRotation = true;
                rb.mass = 10f;
            }
        }
    }
}
