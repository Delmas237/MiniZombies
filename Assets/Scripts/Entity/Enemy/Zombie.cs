using UnityEngine;

namespace EnemyLib
{
    public class Zombie : Enemy
    {
        protected override void Start()
        {
            base.Start();

            HealthController.Died += Death;
        }

        protected void Death()
        {
            if (GetComponent<Rigidbody>() == false)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();

                rb.freezeRotation = true;
                rb.mass = 10f;
            }

            Agent.enabled = false;
            AttackController.IsAttack = false;

            enabled = false;
        }
    }
}
