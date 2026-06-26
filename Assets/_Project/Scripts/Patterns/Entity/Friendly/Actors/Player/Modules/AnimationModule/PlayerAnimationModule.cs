using System;
using System.Collections;
using UnityEngine;
using Weapons;

namespace Entity.Friendly.Player
{
    [Serializable]
    public class PlayerAnimationModule : IEntityModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private float _idleTransitionDelay = 0.5f;
        private Coroutine _idleTransitionCor;

        private Animator _animator;

        private IEntityHealthModule _healthModule;
        private IPlayerInputModule _inputModule;
        private IEntityWeaponModule _weaponModule;

        public bool Enabled { get => _enabled; set => _enabled = value; }

        public void Initialize(Animator animator, IEntityHealthModule healthModule, IPlayerInputModule inputModule, IEntityWeaponModule weaponModule)
        {
            _animator = animator;

            _healthModule = healthModule;
            _inputModule = inputModule;
            _weaponModule = weaponModule;

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
            if (_healthModule.Health > 0 && !_inputModule.IsTraking && !_inputModule.HasMoveInput)
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

            if (_inputModule.HasMoveInput)
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
                if (_healthModule.Health > 0 && !_inputModule.IsTraking)
                    _animator.SetBool("Idle", !_inputModule.HasMoveInput);
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
