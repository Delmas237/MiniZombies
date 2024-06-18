using UnityEngine;
using System.Collections;
using EventBusLib;

namespace LightLib
{
    public class LampPost : MonoBehaviour
    {
        private Light _lamp;

        private void Start()
        {
            _lamp = GetComponentInChildren<Light>();

            EventBus.Subscribe<TimesOfDayChangedEvent>(Controller);
        }
        private void OnDestroy()
        {
            EventBus.Unsubscribe<TimesOfDayChangedEvent>(Controller);
        }

        private void Controller(TimesOfDayChangedEvent changedEvent)
        {
            StartCoroutine(ControllerCor(changedEvent.TimesOfDay));
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
