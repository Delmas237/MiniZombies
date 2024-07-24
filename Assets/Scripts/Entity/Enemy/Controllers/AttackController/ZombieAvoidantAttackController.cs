using System;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class ZombieAvoidantAttackController : ZombieShooterAttackController
    {
        public void Initialize(IEnemyMoveController moveController, Transform transform)
        {
            _moveController = moveController;
            _transform = transform;
        }

        public override void UpdateState()
        {
            bool targetDied = _moveController.Target.HealthController.Health <= 0;
            bool destinationCompleted = Vector3.Distance(_transform.position, _moveController.Agent.destination) < 1f;

            if (!IsAttack && !targetDied)
            {
                if (destinationCompleted)
                    GetIntoPosition();
                else
                    GetOutPosition();
            }
            else if (!destinationCompleted)
            {
                GetOutPosition();
            }
        }
    }
}
