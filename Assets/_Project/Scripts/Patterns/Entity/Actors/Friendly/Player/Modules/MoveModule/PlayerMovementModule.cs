using Entity;
using Entity.Hostile;
using EventBusLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerMovementModule : IPlayerMovementModule
    {
        [SerializeField] private float _defaultSpeed = 3.65f;
        [SerializeField] private float _rotationSpeed = 13f;
        [SerializeField] private float _timeToUpdateClosestEnemy = 0.35f;

        private bool _controllable = true;
        private Rigidbody _rigidbody;
        private Transform _transform;

        private IHostile _closestEnemy;
        private Coroutine _closestEnemyCoroutine;
        private IEntityWeaponModule _weaponModule;

        public float DefaultSpeed => _defaultSpeed;
        public IHostile ClosestEnemy => _closestEnemy;
        public Rigidbody Rigidbody => _rigidbody;

        public void Initialize(Transform transform, Rigidbody rigidbody, IEntityWeaponModule weaponModule)
        {
            _transform = transform;
            _rigidbody = rigidbody;
            _weaponModule = weaponModule;

            EventBus.Subscribe<GameOverEvent>(SetControllableFalse);
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);

            _closestEnemyCoroutine = CoroutineHelper.StartRoutine(UpdateClosestEnemy());
        }
        private void Unsubscribe(GameExitEvent exitEvent)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            EventBus.Unsubscribe<GameOverEvent>(SetControllableFalse);
        }

        private void SetControllableFalse(IEvent e)
        {
            _controllable = false;
        }


        public void Move(Vector2 direction)
        {
            if (!_controllable)
                return;

            if (direction.sqrMagnitude > 1f)
                direction.Normalize();

            direction *= _defaultSpeed;
            _rigidbody.linearVelocity = new Vector3(direction.x, _rigidbody.linearVelocity.y, direction.y);
        }

        public void RotateToDirection(Vector3 direction)
        {
            if (!_controllable)
                return;

            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, Quaternion.LookRotation(direction), _rotationSpeed * Time.deltaTime);
        }

        private IEnumerator UpdateClosestEnemy()
        {
            while (true)
            {
                IReadOnlyList<IHostile> closestEnemies = Spawner<IHostile>.ObjectsOnScene;

                bool enemyInRange = false;
                IHostile closestEnemy = null;
                if (closestEnemies.Count > 0)
                {
                    Vector3 position = _weaponModule.CurrentGun.transform.position;
                    List<Transform> enemiesTransform = closestEnemies.Select(e => e.Transform).ToList();
                    Transform transform = ComponentSearcher<Transform>.Closest(position, enemiesTransform);
                    closestEnemy = closestEnemies[enemiesTransform.IndexOf(transform)];

                    float distanceToEnemy = Vector3.Distance(position, closestEnemy.Transform.position);
                    enemyInRange = distanceToEnemy <= _weaponModule.CurrentGun.Distance;
                }
                _closestEnemy = enemyInRange ? closestEnemy : null;

                yield return new WaitForSeconds(_timeToUpdateClosestEnemy);
            }
        }

        public void OnDestroy()
        {
            CoroutineHelper.StopRoutine(_closestEnemyCoroutine);
        }
    }
}
