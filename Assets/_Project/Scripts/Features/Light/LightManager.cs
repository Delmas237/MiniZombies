using EventBusLib;
using System.Collections;
using UnityEngine;

namespace LightLib
{
    public class LightManager : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 0.02f;
        [Space(10)]
        [SerializeField] private Vector3 _dayRotation;
        [SerializeField] private Vector3 _nightRotation;

        private Light _generalLight;
        private TimesOfDay _timesOfDay;

        private void Start()
        {
            _generalLight = GetComponent<Light>();

            EventBus.Subscribe<TimesOfDayChangedEvent>(SetTimesOfDay);
        }

        private void Update()
        {
#if UNITY_EDITOR
            Cheats();
#endif
        }

        private void Cheats()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                EventBus.Invoke(new TimesOfDayChangedEvent(TimesOfDay.Day));

            if (Input.GetKeyDown(KeyCode.X))
                EventBus.Invoke(new TimesOfDayChangedEvent(TimesOfDay.Night));
        }

        private void SetTimesOfDay(TimesOfDayChangedEvent changedEvent)
        {
            _timesOfDay = changedEvent.TimesOfDay;

            StopAllCoroutines();
            switch (_timesOfDay)
            {
                case TimesOfDay.Day:
                    StartCoroutine(RotateTo(_dayRotation));
                    break;
                case TimesOfDay.Night:
                    StartCoroutine(RotateTo(_nightRotation));
                    break;
            }
        }

        private IEnumerator RotateTo(Vector3 vector)
        {
            while (Vector3.Distance(_generalLight.transform.eulerAngles, vector) % 360 > 1f)
            {
                _generalLight.transform.rotation = Quaternion.Lerp(
                    transform.rotation, Quaternion.Euler(vector), _rotationSpeed);
                yield return null;
            }
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<TimesOfDayChangedEvent>(SetTimesOfDay);
        }
    }
}
