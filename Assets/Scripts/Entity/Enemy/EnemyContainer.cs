using UnityEngine;
using Weapons;

namespace EnemyLib
{
    public abstract class EnemyContainer : MonoBehaviour, IEnemy
    {
        [field: Header("Controllers")]
        [field: SerializeField] public HealthController HealthController { get; set; }
        [field: SerializeField] public EnemyAttackController AttackController { get; set; }
        [field: SerializeField] public EnemyMoveController MoveController { get; set; }
        [field: SerializeField] public EnemyAnimationController AnimationController { get; set; }
        
        [field: Header("Modules")]
        [field: SerializeField] public DropAmmoAfterDeathModule DropAmmoAfterDeathModule { get; set; }

        protected virtual void Start()
        {
            HealthController.Initialize();
            AnimationController.Initialize(this);
            AttackController.Initialize(this);

            DropAmmoAfterDeathModule.Initialize(this, transform);
        }

        protected virtual void Update()
        {
            MoveController.Move();
            AnimationController.MoveAnim();

            AnimationController.AttackAnim();
        }

        private void OnCollisionEnter(Collision collision)
        {
            AttackController.OnCollisionEnter(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            AttackController.OnCollisionExit(collision);
        }

        private void DealDamage() => AttackController.DealDamage();
    }
}
