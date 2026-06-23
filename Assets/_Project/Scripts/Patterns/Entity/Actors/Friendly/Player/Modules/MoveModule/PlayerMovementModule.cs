using EventBusLib;
using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerMovementModule : IPlayerMovementModule
    {
        [SerializeField] private float _defaultSpeed = 3.65f;
        [SerializeField] private float _rotationSpeed = 13f;

        private bool _controllable = true;
        private Rigidbody _rigidbody;
        private Transform _transform;

        public float DefaultSpeed => _defaultSpeed;
        public Rigidbody Rigidbody => _rigidbody;

        public void Initialize(Transform transform, Rigidbody rigidbody)
        {
            _transform = transform;
            _rigidbody = rigidbody;

            EventBus.Subscribe<GameOverEvent>(SetControllableFalse);
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
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
    }
}
