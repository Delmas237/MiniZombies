using UnityEngine;
using System.Collections;

namespace LightLib
{
    public class LampPost : MonoBehaviour
    {
        private Light lamp;

        private void Start()
        {
            lamp = GetComponentInChildren<Light>();

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
                    lamp.enabled = false;
                    break;

                case TimesOfDay.Night:
                    yield return new WaitForSeconds(3);
                    lamp.enabled = true;
                    break;
            }
        }
    }
}
