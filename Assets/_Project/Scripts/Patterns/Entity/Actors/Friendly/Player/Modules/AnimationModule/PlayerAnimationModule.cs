using Entity;
using System;
using System.Collections;
using UnityEngine;
using Weapons;

namespace Player
{
    [Serializable]
    public class PlayerAnimationModule
    {
        [SerializeField] private float _idleTransitionDelay = 0.5f;
        private Coroutine _idleTransitionCor;

        private Animator _animator;

        private IEntityHealthModule _healthModule;
        private IEntityWeaponModule _weaponModule;
        private IPlayerMovementModule _moveModule;

        public void Initialize(IEntityHealthModule healthModule, IEntityWeaponModule weaponModule, IPlayerMovementModule moveModule, 
            Animator animator)
        {
            _healthModule = healthModule;
            _weaponModule = weaponModule;
            _moveModule = moveModule;

            _animator = animator;

            _weaponModule.GunChanged += CurrentGunAnim;
            _healthModule.IsOver += OnHealhIsOver;
            _healthModule.IsOver += DeathAnim;

            _animator.SetBool("Idle", true);
        }
        private void OnHealhIsOver()
        {
            _weaponModule.GunChanged -= CurrentGunAnim;
            _healthModule.IsOver -= OnHealhIsOver;
            _healthModule.IsOver -= DeathAnim;
        }

        public void MoveAnim()
        {
            if (_healthModule.Health > 0 && !_moveModule.IsTraking && !_moveModule.IsMoving)
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

            if (_moveModule.IsMoving)
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
                if (_healthModule.Health > 0 && !_moveModule.IsTraking)
                    _animator.SetBool("Idle", !_moveModule.IsMoving);
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
