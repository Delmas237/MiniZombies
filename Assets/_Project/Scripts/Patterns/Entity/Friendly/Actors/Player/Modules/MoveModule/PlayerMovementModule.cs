using EventBusLib;
using System;
using UnityEngine;

namespace Entity.Friendly.Player
{
    [Serializable]
    public class PlayerMovementModule : IPlayerMovementModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private float _defaultSpeed = 3.65f;
        [SerializeField] private float _rotationSpeed = 13f;

        private Rigidbody _rigidbody;
        private Transform _transform;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public float DefaultSpeed => _defaultSpeed;
        public Rigidbody Rigidbody => _rigidbody;

        public void Initialize(Transform transform, Rigidbody rigidbody)
        {
            _transform = transform;
            _rigidbody = rigidbody;

            EventBus.Subscribe<GameOverEvent>(Disable);
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent exitEvent)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
            EventBus.Unsubscribe<GameOverEvent>(Disable);
        }

        private void Disable(IEvent e)
        {
            _enabled = false;
        }

        public void Move(Vector2 direction)
        {
            if (!_enabled)
                return;

            if (direction.sqrMagnitude > 1f)
                direction.Normalize();

            direction *= _defaultSpeed;
            _rigidbody.linearVelocity = new Vector3(direction.x, _rigidbody.linearVelocity.y, direction.y);
        }

        public void RotateToDirection(Vector3 direction)
        {
            if (!_enabled)
                return;

            _transform.rotation = Quaternion.Lerp(
                _transform.rotation, Quaternion.LookRotation(direction), _rotationSpeed * Time.deltaTime);
        }
    }
}
