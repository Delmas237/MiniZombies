using System;
using System.Collections;
using UnityEngine;

namespace Entity.Friendly.Turret
{
    [Serializable]
    public class TurretInstallModule : ITurretInstallModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private Motion _installMotion;

        private bool _isInstalled;
        private Coroutine _installCoroutine;

        public event Action InstallStarted;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (!_enabled)
                        StopInstallImmediately();
                }
            }
        }
        public bool IsInstalled => _isInstalled;


        public void Install()
        {
            if (!Enabled)
                return;

            if (_installCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_installCoroutine);
                _installCoroutine = null;
            }

            _installCoroutine = CoroutineHelper.StartRoutine(InstallCoroutine());
        }

        private IEnumerator InstallCoroutine()
        {
            _isInstalled = false;
            InstallStarted?.Invoke();

            float duration = _installMotion.averageDuration;
            yield return new WaitForSeconds(duration);

            if (!Enabled)
                yield break;

            _isInstalled = true;
            _installCoroutine = null;
        }

        public void StopInstallImmediately()
        {
            _isInstalled = false;

            if (_installCoroutine != null)
            {
                CoroutineHelper.StopRoutine(_installCoroutine);
                _installCoroutine = null;
            }
        }

        public void Dispose()
        {
            StopInstallImmediately();
        }
    }
}
