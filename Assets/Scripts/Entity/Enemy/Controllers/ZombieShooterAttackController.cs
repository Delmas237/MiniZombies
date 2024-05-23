using System;
using System.Collections;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class ZombieShooterAttackController : IEnemyAttackController
    {
        public bool IsAttack { get; set; }

        [SerializeField] private int _damage = 15;
        public int Damage => _damage;

        [SerializeField] private float _cooldown = 1f;

        private IEnemyMoveController _moveController;
        private IWeaponsController _weaponsController;
        private EnemyAnimationController _animationController;
        private Transform _transform;

        public void Initialize(IEnemyMoveController moveController, IWeaponsController weaponsController, EnemyAnimationController animationController,
            Transform transform)
        {
            _moveController = moveController;
            _weaponsController = weaponsController;
            _animationController = animationController;
            _transform = transform;

            _weaponsController.CurrentGun.Damage = _damage;
            _weaponsController.CurrentGun.Cooldown = _cooldown;
            _animationController.AttackSpeedX /= _cooldown;
        }

        public void UpdateState()
        {
            float distanceToTarget = Vector3.Distance(_moveController.Target.Transform.position, _transform.position);

            if (!IsAttack)
            {
                if (distanceToTarget < _weaponsController.CurrentGun.Distance / 2)
                {
                    GetIntoPosition();
                }
            }
            else
            {
                if (distanceToTarget > _weaponsController.CurrentGun.Distance || _moveController.Target.HealthController.Health <= 0)
                {
                    GetOutPosition();
                }
            }
        }

        private void GetIntoPosition()
        {
            IsAttack = true;
            CoroutineHelper.StartRoutine(Shoot());

            if (_moveController.Agent.enabled)
                _moveController.Agent.isStopped = true;
        }
        private IEnumerator Shoot()
        {
            while (IsAttack && _moveController.Target.HealthController.Health > 0)
            {
                _weaponsController.PullTrigger();
                yield return new WaitForSeconds(_cooldown + 0.01f);
            }
        }

        private void GetOutPosition()
        {
            IsAttack = false;

            if (_moveController.Agent.enabled)
                _moveController.Agent.isStopped = false;
        }
    }
}
