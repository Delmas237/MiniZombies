using EventBusLib;
using LightLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WavesLib
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField, Min(1)] private int _wavesAmount = 100;
        [SerializeField, Min(0)] private float _timeBtwWaves = 10;
        [Space(10)]
        [SerializeField, Min(1)] private int _minWaveForNight = 2;
        [SerializeField, Min(0)] private float _changeDayTimeDelay = 1;
        [Space(10)]
        [SerializeField] private List<WavePreset> _presets;

        private readonly List<Wave> _waves = new List<Wave>();

        public Wave CurrentWave => _waves[CurrentWaveIndex];
        public int CurrentWaveIndex => _waves.Count - 1;
        public int WavesAmount => _wavesAmount;

        private void Start()
        {
            _waves.Add(ConstructWave());
            StartWave();
        }

        private void StartWave()
        {
            EventBus.Invoke(new WaveStartedEvent(CurrentWave, CurrentWaveIndex + 1));
            StartCoroutine(CurrentWave.State.Control(WaitTransition));
            CurrentWave.State.UpdateUIInfo();
        }

        private IEnumerator WaitTransition()
        {
            yield return new WaitWhile(() => Spawner<IEnemy>.ObjectsOnScene.Count > 0);
            CoroutineHelper.StartRoutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (CurrentWaveIndex + 1 >= _wavesAmount)
            {
                EventBus.Invoke(new AllWavesFinishedEvent(CurrentWave, CurrentWaveIndex + 1));
                yield break;
            }
            EventBus.Invoke(new WaveFinishedEvent(CurrentWave, CurrentWaveIndex + 1));
            
            _waves.Add(ConstructWave());

            yield return StartCoroutine(ChangeDayTime());
            yield return new WaitForSeconds(_timeBtwWaves);

            StartWave();
        }
        private IEnumerator ChangeDayTime()
        {
            yield return new WaitForSeconds(_changeDayTimeDelay);

            if (CurrentWaveIndex < _minWaveForNight)
                CurrentWave.TimesOfDay = TimesOfDay.Day;

            if (CurrentWave.TimesOfDay != _waves[CurrentWaveIndex - 1].TimesOfDay)
                EventBus.Invoke(new TimesOfDayChangedEvent(CurrentWave.TimesOfDay));
        }

        private Wave ConstructWave()
        {
            int rnd = Random.Range(0, _presets.Count);
            Wave wave = _presets[rnd].Construct(CurrentWaveIndex + 1);
            return wave;
        }
    }
}
