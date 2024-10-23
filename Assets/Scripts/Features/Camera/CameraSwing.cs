using UnityEngine;

namespace CameraLib
{
    public class CameraSwing : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationAngle;
        [SerializeField] private Vector3 _rotationSpeed;
        private Vector3 _startRotation;

        private Vector3 _rotationDir = Vector3.one;

        private void Start()
        {
            _startRotation = transform.eulerAngles;
        }

        private void FixedUpdate()
        {
            Rotate();
        }

        private void Rotate()
        {
            RotateInDir(ref _rotationDir.x, transform.eulerAngles.x, _rotationAngle.x, _startRotation.x);
            RotateInDir(ref _rotationDir.y, transform.eulerAngles.y, _rotationAngle.y, _startRotation.y);
            RotateInDir(ref _rotationDir.z, transform.eulerAngles.z, _rotationAngle.z, _startRotation.z);

            Quaternion rotation = Quaternion.Euler(
                _rotationDir.x * _rotationSpeed.x,
                _rotationDir.y * _rotationSpeed.y,
                _rotationDir.z * _rotationSpeed.z);

            transform.rotation *= rotation;
        }

        private void RotateInDir(ref float rotateDir, float eulerAngle, float rotationAngle, float startRotation)
        {
            float rotation = eulerAngle > 180 ? eulerAngle - 360 : eulerAngle;

            if (rotation > startRotation + rotationAngle / 2)
                rotateDir = -1;
            if (rotation < startRotation - rotationAngle / 2)
                rotateDir = 1;
        }
    }
}
