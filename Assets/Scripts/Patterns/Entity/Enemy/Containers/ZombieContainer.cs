using UnityEngine;
using Weapons;

namespace EnemyLib
{
    public abstract class ZombieContainer : MonoBehaviour, IEnemy
    {
        [Header("Base Modules")]
        [SerializeField] protected HealthModule _healthModule;
        [SerializeField] protected EnemyAnimationModule _animationModule;
        [SerializeField] protected EntityAudioModule _audioModule;

        [Header("Base Additional Modules")]
        [SerializeField] protected DelayedDisableEntityModule _delayedDisableModule;
        [SerializeField] protected DropAmmoAfterDeathModule _dropAmmoAfterDeathModule;

        public IHealthModule HealthModule => _healthModule;
        public DelayedDisableEntityModule DelayedDisableModule => _delayedDisableModule;
        public DropAmmoAfterDeathModule DropAmmoAfterDeathModule => _dropAmmoAfterDeathModule;
        public Transform Transform => transform;

        public abstract IEnemyMoveModule MoveModule { get; }
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
                rb.velocity /= 2;
            }
        }

        protected virtual void OnDestroy()
        {
            HealthModule.IsOver -= OnHealhIsOver;
        }
    }
}
