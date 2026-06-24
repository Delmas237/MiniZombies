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
    public class FriendlyTargetModule : IEntityTargetModule
    {
        [SerializeField] private float _updateTargetTime = 0.35f;

        private Coroutine _findClosestCoroutine;

        private IEntityWeaponModule _weaponModule;

        public bool IsFindingTarget { get; set; } = true;
        public IEntity Target { get; set; }

        public void Initialize(IEntityWeaponModule weaponModule)
        {
            _weaponModule = weaponModule;

            _findClosestCoroutine = CoroutineHelper.StartRoutine(FindClosest());
            EventBus.Subscribe<GameOverEvent>(OnGameOver);
        }
        private void OnGameOver(GameOverEvent gameOverEvent)
        {
            IsFindingTarget = false;
            Target = null;
            CoroutineHelper.StopRoutine(_findClosestCoroutine);
            _findClosestCoroutine = null;
        }

        private IEnumerator FindClosest()
        {
            while (true)
            {
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

        public void OnDestroy()
        {
            if (_findClosestCoroutine != null)
                CoroutineHelper.StopRoutine(_findClosestCoroutine);
            EventBus.Unsubscribe<GameOverEvent>(OnGameOver);
        }
    }
}
