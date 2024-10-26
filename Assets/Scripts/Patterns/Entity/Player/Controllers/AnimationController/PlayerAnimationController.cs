using System;
using System.Collections;
using UnityEngine;
using Weapons;

namespace PlayerLib
{
    [Serializable]
    public class PlayerAnimationController
    {
        [SerializeField] private float _idleTransitionDelay = 0.5f;
        private Coroutine _idleTransitionCor;

        private Animator _animator;

        private IHealthController _healthController;
        private IWeaponsController _weaponsController;
        private IPlayerMoveController _moveController;

        public void Initialize(IHealthController healthController, IWeaponsController weaponsController, IPlayerMoveController moveController, 
            Animator animator)
        {
            _healthController = healthController;
            _weaponsController = weaponsController;
            _moveController = moveController;

            _animator = animator;

            _weaponsController.GunChanged += CurrentGunAnim;
            _healthController.IsOver += OnHealhIsOver;
            _healthController.IsOver += DeathAnim;

            _animator.SetBool("Idle", true);
        }
        private void OnHealhIsOver()
        {
            _weaponsController.GunChanged -= CurrentGunAnim;
            _healthController.IsOver -= OnHealhIsOver;
            _healthController.IsOver -= DeathAnim;
        }

        public void MoveAnim()
        {
            if (_healthController.Health > 0 && !_moveController.IsTraking && !_moveController.IsMoving)
            {
                if (_idleTransitionCor == null && !_animator.GetBool("Idle"))
                    _idleTransitionCor = CoroutineHelper.StartRoutine(IdleCor());
            }
            else
            {
                _animator.SetBool("Idle", false);

                if (_idleTransitionCor != null)
                {
                    CoroutineHelper.StopRoutine(_idleTransitionCor);
                    _idleTransitionCor = null;
                }
            }

            if (_moveController.IsMoving)
            {
                _animator.SetFloat("SpeedPistol", 1);
                _animator.SetFloat("Speed", 1.7f);
            }
            else
            {
                _animator.SetFloat("SpeedPistol", 0);
                _animator.SetFloat("Speed", 0);
            }
        }
        private IEnumerator IdleCor()
        {
            yield return new WaitForSeconds(_idleTransitionDelay);

            if (_animator != null)
            {
                if (_healthController.Health > 0 && !_moveController.IsTraking)
                    _animator.SetBool("Idle", !_moveController.IsMoving);
                else
                    _animator.SetBool("Idle", false);
            }

            _idleTransitionCor = null;
        }

        private void CurrentGunAnim(Gun gun)
        {
            _animator.SetBool("PistolInHands", gun.Type == GunType.Pistol);
        }

        private void DeathAnim() => _animator.SetBool("Died", true);
    }
}
