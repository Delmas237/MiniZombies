using UnityEngine;
using Weapons;

namespace EnemyLib
{
    public abstract class ZombieContainer : MonoBehaviour, IEnemy
    {
        [Header("Base Controllers")]
        [SerializeField] protected HealthController _healthController;
        public IHealthController HealthController => _healthController;

        public abstract IEnemyMoveController MoveController { get; }
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
            HealthController.IsOver += OnHealhIsOver;
        }

        protected virtual void OnEnable()
        {
            _animationController.UpdateData();
        }

        protected virtual void OnHealhIsOver()
        {
            if (GetComponent<Rigidbody>() == false)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();

                rb.freezeRotation = true;
                rb.mass = 10f;
                rb.velocity /= 2;
            }
        }

        protected virtual void OnDestroy()
        {
            HealthController.IsOver -= OnHealhIsOver;
        }
    }
}
