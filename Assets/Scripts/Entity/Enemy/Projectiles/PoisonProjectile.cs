using System;
using UnityEngine;

namespace EnemyLib
{
    public class PoisonProjectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        [SerializeField] private AnimationCurve _flight—urve;

        public IInstanceProvider<ParticleSystem> EffectProvider { get; set; }

        private Vector3 _start;
        private Vector3 _target;

        public void Initialize(Vector3 start, Vector3 target)
        {
            _start = start;
            _target = target;
            
            transform.position = _start;
        }

        private void Update()
        {
            Vector3 vector = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);

            Vector3 currentDiff = new Vector3(Math.Abs(transform.position.x - _start.x), 0, Math.Abs(transform.position.z - _start.z));
            Vector3 maxDiff = new Vector3(Math.Abs(_start.x - _target.x), 0, Math.Abs(_start.z - _target.z));

            float curveValue = _flight—urve.Evaluate((currentDiff.x + currentDiff.z) / (maxDiff.x + maxDiff.z));

            transform.position = new Vector3(vector.x, curveValue, vector.z);

            if (Vector3.Distance(currentDiff, maxDiff) < 0.5f)
            {
                ParticleSystem particleSystem = EffectProvider.GetInstance();
                particleSystem.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                gameObject.SetActive(false);
            }
        }
    }
}
