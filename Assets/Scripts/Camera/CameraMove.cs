using System;
using UnityEngine;

namespace CameraLib
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smoothness = 0.05f;

        private Vector3 targetDistance;

        private void Start()
        {
            if (target)
                targetDistance = transform.position - target.position;
            else
                throw new NullReferenceException("Camera has no target");
        }

        private void FixedUpdate()
        {
            Vector3 endPos = new Vector3(
                target.position.x + targetDistance.x, transform.position.y,
                target.position.z + targetDistance.z);

            transform.position = Vector3.Lerp(transform.position, endPos, smoothness);
        }
    }
}
