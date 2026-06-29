using Entity.Hostile;
using EventBusLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity.Friendly
{
    [Serializable]
    public class FriendlyTargetModule : IEntityTargetModule, IDisposable
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private float _updateTargetTime = 0.35f;

        private Coroutine _findClosestCor;

        private IEntityWeaponModule _weaponModule;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (!_enabled)
                        StopFindingTargetImmediately();
                }
            }
        }

        public bool IsFindingTarget { get; set; } = true;
        public IEntity Target { get; set; }

        public void Initialize(IEntityWeaponModule weaponModule)
        {
            _weaponModule = weaponModule;

            _findClosestCor = CoroutineHelper.StartRoutine(FindClosest());
            EventBus.Subscribe<GameOverEvent>(OnGameOver);
        }

        private void OnGameOver(GameOverEvent gameOverEvent)
        {
            IsFindingTarget = false;
            Target = null;

            if (_findClosestCor != null)
            {
                CoroutineHelper.StopRoutine(_findClosestCor);
                _findClosestCor = null;
            }
        }

        private IEnumerator FindClosest()
        {
            while (true)
            {
                if (!_enabled)
                {
                    yield return new WaitForSeconds(_updateTargetTime);
                    continue;
                }

                if (IsFindingTarget)
                {
                    IReadOnlyList<IHostile> targets = Spawner<IHostile>.ObjectsOnScene;

                    bool isTargetInRange = false;
                    IHostile closestTarget = null;
                    if (targets.Count > 0)
                    {
                        Vector3 position = _weaponModule.CurrentGun.transform.position;
                        List<Transform> targetsTransform = targets.Select(e => e.Transform).ToList();
                        Transform transform = ComponentSearcher<Transform>.Closest(position, targetsTransform);
                        closestTarget = targets[targetsTransform.IndexOf(transform)];

                        float distanceToTarget = Vector3.Distance(position, closestTarget.Transform.position);
                        isTargetInRange = distanceToTarget <= _weaponModule.CurrentGun.Distance;
                    }
                    Target = isTargetInRange ? closestTarget : null;
                }

                yield return new WaitForSeconds(_updateTargetTime);
            }
        }

        private void StopFindingTargetImmediately()
        {
            IsFindingTarget = false;
            Target = null;

            if (_findClosestCor != null)
            {
                CoroutineHelper.StopRoutine(_findClosestCor);
                _findClosestCor = null;
            }
        }

        public void Dispose()
        {
            StopFindingTargetImmediately();
            EventBus.Unsubscribe<GameOverEvent>(OnGameOver);
        }
    }
}