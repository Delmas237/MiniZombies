using UnityEngine;

namespace Entity.Hostile
{
    public abstract class ZombieEntity : MonoBehaviour, IHostile
    {
        [Header("Base Modules")]
        [SerializeField] protected EntityHealthModule _healthModule;
        [SerializeField] protected EnemyTargetModule _targetModule;
        [SerializeField] protected EnemyAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;

        [Header("Base Additional Modules")]
        [SerializeField] protected EntityDelayedDisableModule _delayedDisableModule;
        [SerializeField] protected EntityDropAmmoOnDeathModule _dropAmmoAfterDeathModule;

        public IEntityHealthModule HealthModule => _healthModule;
        public IEntityTargetModule TargetModule => _targetModule;
        public EntityDelayedDisableModule DelayedDisableModule => _delayedDisableModule;
        public EntityDropAmmoOnDeathModule DropAmmoAfterDeathModule => _dropAmmoAfterDeathModule;
        public Transform Transform => transform;

        public abstract IEnemyMovementModule MovementModule { get; }
        public abstract IEnemyAttackModule AttackModule { get; }

        protected virtual void Awake()
        {
            HealthModule.IsOver += OnHealhIsOver;
        }

        protected virtual void OnEnable()
        {
            _animationModule.UpdateData();
        }

        protected virtual void OnHealhIsOver()
        {
            if (GetComponent<Rigidbody>() == false)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();

                rb.freezeRotation = true;
                rb.mass = 10f;
                rb.linearVelocity /= 2;
            }
        }

        protected virtual void OnDestroy()
        {
            HealthModule.IsOver -= OnHealhIsOver;
        }
    }
}
