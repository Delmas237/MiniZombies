using System;
using UnityEngine;

namespace LightLib
{
    public class LightManager : MonoBehaviour
    {
        private Light generalLight;

        [SerializeField] private float rotationSpeed = 0.02f;

        [SerializeField] private Vector3 dayRotation;
        [SerializeField] private Vector3 nightRotation;

        public static event Action<TimesOfDay> TimesOfDayChanged;

        private TimesOfDay timesOfDay;

        private void Start()
        {
            generalLight = GetComponent<Light>();
        }

        private void Update()
        {
            switch (timesOfDay)
            {
                case TimesOfDay.Day:
                    RotateSunTo(dayRotation);
                    break;
                case TimesOfDay.Night:
                    RotateSunTo(nightRotation);
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
            this.timesOfDay = timesOfDay;
            TimesOfDayChanged?.Invoke(timesOfDay);
        }

        private void RotateSunTo(Vector3 vector3)
        {
            generalLight.transform.rotation = Quaternion.Lerp(
                transform.rotation, Quaternion.Euler(vector3), rotationSpeed);
        }
    }
}
