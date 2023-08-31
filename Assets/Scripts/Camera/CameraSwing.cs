using UnityEngine;

namespace CameraLib
{
    public class CameraSwing : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationAngle;
        [SerializeField] private Vector3 rotationSpeed;
        private Vector3 startRotation;

        private Vector3 rotateDir = Vector3.one;

        private void Start()
        {
            startRotation = transform.eulerAngles;
        }

        private void FixedUpdate()
        {
            Rotate();
        }

        private void Rotate()
        {
            RotateInDir(ref rotateDir.x, transform.eulerAngles.x, rotationAngle.x, startRotation.x);
            RotateInDir(ref rotateDir.y, transform.eulerAngles.y, rotationAngle.y, startRotation.y);
            RotateInDir(ref rotateDir.z, transform.eulerAngles.z, rotationAngle.z, startRotation.z);

            Quaternion rotation = Quaternion.Euler(
                rotateDir.x * rotationSpeed.x,
                rotateDir.y * rotationSpeed.y,
                rotateDir.z * rotationSpeed.z);

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
