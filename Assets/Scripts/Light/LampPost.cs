using UnityEngine;
using System.Collections;

namespace LightLib
{
    public class LampPost : MonoBehaviour
    {
        private Light _lamp;

        private void Start()
        {
            _lamp = GetComponentInChildren<Light>();

            LightManager.TimesOfDayChanged += Controller;
        }
        private void OnDestroy()
        {
            LightManager.TimesOfDayChanged -= Controller;
        }

        private void Controller(TimesOfDay timesOfDay)
        {
            StartCoroutine(ControllerCor(timesOfDay));
        }
        private IEnumerator ControllerCor(TimesOfDay timesOfDay)
        {
            switch (timesOfDay)
            {
                case TimesOfDay.Day:
                    yield return new WaitForSeconds(0.3f);
                    _lamp.enabled = false;
                    break;

                case TimesOfDay.Night:
                    yield return new WaitForSeconds(3);
                    _lamp.enabled = true;
                    break;
            }
        }
    }
}
