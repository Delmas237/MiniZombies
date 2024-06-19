using EventBusLib;
using LightLib;
using System.Collections;
using TMPro;
using UnityEngine;

namespace EnemyLib
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField] private float _timeBtwWaves = 10;

        [Tooltip("Minimum currentWave for night version")]
        [SerializeField, Min(1)] private int _minWaveForNight = 2;
        [Space(10)]
        [SerializeField] private EnemySpawner _spawnManager;
        [SerializeField] private TextMeshProUGUI _waveUIInfo;

        private readonly Wave[] _waves = new Wave[100];

        private Wave _currentWave;
        public static int CurrentWaveIndex { get; private set; } = 0;

        public int CurrentWaveEnemiesDied => _spawnManager.EnemiesDied - _enemiesDiedBeforeWave;
        private int _enemiesDiedBeforeWave;

        private void Start()
        {
            WavesConstruct();

            StartWave();
        }
        private void OnDestroy()
        {
            CurrentWaveIndex = 0;
        }

        private void Update()
        {
            UpdateUIInfo();
        }

        private void UpdateUIInfo()
        {
            switch (_currentWave.Type)
            {
                case WaveType.EnemyAmount:
                    if (_currentWave.EnemyAmount <= 0 && EnemySpawner.EnemiesOnScene.Count > 0)
                    {
                        _waveUIInfo.text = EnemySpawner.EnemiesOnScene.Count.ToString();
                    }
                    else if (_currentWave.EnemyAmount > 0 || EnemySpawner.EnemiesOnScene.Count > 0)
                    {
                        _waveUIInfo.text = (_currentWave.StartEnemyAmount - CurrentWaveEnemiesDied).ToString();
                    }
                    else
                    {
                        _waveUIInfo.text = string.Empty;
                    }
                    break;

                case WaveType.Duration:
                    if (_currentWave.DurationSec > 0)
                    {
                        _waveUIInfo.text = Utilities.Watch((int)_currentWave.DurationSec);
                    }
                    else
                    {
                        _waveUIInfo.text = string.Empty;
                    }
                    break;
            }
        }

        private void WavesConstruct()
        {
            for (int i = 0; i < _waves.Length; i++)
            {
                WaveType waveType = (WaveType)UnityEngine.Random.Range(0, 2);

                switch (waveType)
                {
                    case WaveType.EnemyAmount:
                        _waves[i] = new Wave(
                            amount: 12 + i * 4,
                            spawnSpeed: 1.2f - i * 0.008f);
                        break;

                    case WaveType.Duration:
                        _waves[i] = new Wave(
                            durationSec: 16 + i * 2,
                            spawnSpeed: 1.5f - i * 0.012f);
                        break;
                }

                if (i < _minWaveForNight)
                    _waves[i].TimesOfDay = TimesOfDay.Day;
            }
        }
        
        private IEnumerator WaveController()
        {
            switch (_currentWave.Type)
            {
                case WaveType.EnemyAmount:
                    if (_currentWave.EnemyAmount > 0)
                    {
                        yield return new WaitForSeconds(_currentWave.SpawnSpeed);
                        _currentWave.EnemyAmount--;

                        StartCoroutine(WaveController());
                    }
                    else
                    {
                        StartCoroutine(NextWaveOrRecheck());
                    }
                    break;

                case WaveType.Duration:
                    if (_currentWave.DurationSec > 0)
                    {
                        yield return new WaitForSeconds(1);
                        _currentWave.DurationSec--;

                        StartCoroutine(WaveController());
                    }
                    else
                    {
                        StartCoroutine(NextWaveOrRecheck());
                    }
                    break;
            }
        }

        private IEnumerator NextWaveOrRecheck()
        {
            if (EnemySpawner.EnemiesOnScene.Count <= 0)
            {
                StartCoroutine(WaveTransition());
            }
            else //all enemies spawned, but not all died
            {
                _spawnManager.IsSpawn = false;

                yield return new WaitForSeconds(1f);
                StartCoroutine(WaveController());
            }
        }

        private IEnumerator WaveTransition()
        {
            _spawnManager.IsSpawn = false;
            yield return new WaitForSeconds(1);

            CurrentWaveIndex++;
            if (CurrentWaveIndex == _waves.Length)
            {
                EventBus.Invoke(new AllWavesFinishedEvent());
                yield break;
            }
            EventBus.Invoke(new WaveFinishedEvent());

            if (_waves[CurrentWaveIndex].TimesOfDay != _waves[CurrentWaveIndex - 1].TimesOfDay)
                EventBus.Invoke(new TimesOfDayChangedEvent(_waves[CurrentWaveIndex].TimesOfDay));

            yield return new WaitForSeconds(_timeBtwWaves);

            StartWave();
        }

        private void StartWave()
        {
            _currentWave = _waves[CurrentWaveIndex];
            _enemiesDiedBeforeWave += CurrentWaveEnemiesDied;

            _spawnManager.Cooldown = _currentWave.SpawnSpeed;

            _spawnManager.IsSpawn = true;
            EventBus.Invoke(new WaveStartedEvent());

            StartCoroutine(WaveController());
        }
    }

    public class Wave
    {
        public readonly WaveType Type;
        public TimesOfDay TimesOfDay = (TimesOfDay)UnityEngine.Random.Range(0, 2);

        public readonly int StartEnemyAmount;
        public int EnemyAmount;

        public float DurationSec;

        public readonly float SpawnSpeed;

        public Wave(int amount, float spawnSpeed)
        {
            Type = WaveType.EnemyAmount;

            EnemyAmount = amount;
            StartEnemyAmount = amount;

            SpawnSpeed = spawnSpeed;
        }
        public Wave(float durationSec, float spawnSpeed)
        {
            Type = WaveType.Duration;

            DurationSec = durationSec;
            SpawnSpeed = spawnSpeed;
        }
    }

    public enum WaveType
    {
        EnemyAmount,
        Duration
    }
}
