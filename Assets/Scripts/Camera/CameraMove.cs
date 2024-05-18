using System;
using UnityEngine;

namespace CameraLib
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _smoothness = 0.1f;

        private Vector3 _targetDistance;

        private void Start()
        {
            if (_target)
                _targetDistance = transform.position - _target.position;
            else
                throw new NullReferenceException("Camera has no target");
        }

        private void FixedUpdate()
        {
            Vector3 endPos = new Vector3(
                _target.position.x + _targetDistance.x, transform.position.y,
                _target.position.z + _targetDistance.z);

            transform.position = Vector3.Lerp(transform.position, endPos, _smoothness);
        }
    }
}
