using UnityEngine;
using Weapons;

namespace EnemyLib
{
    public class ZombieContainer : MonoBehaviour, IEnemy
    {
        [field: Header("Base Controllers")]
        [field: SerializeField] public HealthController HealthController { get; set; }
        [field: SerializeField] public EnemyMoveController MoveController { get; set; }
        [field: SerializeField] public EnemyAnimationController AnimationController { get; set; }
        
        [field: Header("Base Modules")]
        [field: SerializeField] public DropAmmoAfterDeathModule DropAmmoAfterDeathModule { get; set; }

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
