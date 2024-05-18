using UnityEngine;
using Weapons;

namespace EnemyLib
{
    public class ZombieContainer : MonoBehaviour, IEnemy
    {
        [Header("Base Controllers")]
        [SerializeField] protected HealthController _healthController;
        public IHealthController HealthController => _healthController;

        [SerializeField] protected EnemyMoveController _moveController;
        public IEnemyMoveController MoveController => _moveController;

        [SerializeField] protected EnemyAnimationController _animationController;
        public EnemyAnimationController AnimationController => _animationController;

        [field: Header("Base Modules")]
        [SerializeField] protected DropAmmoAfterDeathModule _dropAmmoAfterDeathModule;
        public DropAmmoAfterDeathModule DropAmmoAfterDeathModule => _dropAmmoAfterDeathModule;

        protected virtual void Start()
        {
            HealthController.Died += OnDeath;
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
