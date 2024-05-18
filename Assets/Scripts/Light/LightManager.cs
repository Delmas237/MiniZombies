using System;
using UnityEngine;

namespace LightLib
{
    public class LightManager : MonoBehaviour
    {
        private Light _generalLight;

        [SerializeField] private float _rotationSpeed = 0.02f;

        [SerializeField] private Vector3 _dayRotation;
        [SerializeField] private Vector3 _nightRotation;

        public static event Action<TimesOfDay> TimesOfDayChanged;

        private TimesOfDay _timesOfDay;

        private void Start()
        {
            _generalLight = GetComponent<Light>();
        }

        private void Update()
        {
            switch (_timesOfDay)
            {
                case TimesOfDay.Day:
                    RotateSunTo(_dayRotation);
                    break;
                case TimesOfDay.Night:
                    RotateSunTo(_nightRotation);
                    break;
            }

#if UNITY_EDITOR
            Cheats();
#endif
        }

        private void Cheats()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                SetTimesOfDay(TimesOfDay.Day);

            if (Input.GetKeyDown(KeyCode.X))
                SetTimesOfDay(TimesOfDay.Night);
        }

        public void SetTimesOfDay(TimesOfDay timesOfDay)
        {
            _timesOfDay = timesOfDay;
            TimesOfDayChanged?.Invoke(timesOfDay);
        }

        private void RotateSunTo(Vector3 vector)
        {
            _generalLight.transform.rotation = Quaternion.Lerp(
                transform.rotation, Quaternion.Euler(vector), _rotationSpeed);
        }
    }
}
