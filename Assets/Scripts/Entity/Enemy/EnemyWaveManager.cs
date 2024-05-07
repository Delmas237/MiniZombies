using System.Collections;
using UnityEngine;
using TMPro;
using System;
using LightLib;

namespace EnemyLib
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField] private float timeBtwWaves;

        [Tooltip("Minimum currentWave for night version")]
        [SerializeField] private int minWaveForNight = 2;
        [Space(10)]
        [SerializeField] private EnemySpawnManager spawnManager;
        [SerializeField] private LightManager lightManager;
        [SerializeField] private TextMeshProUGUI waveUIInfo;

        private readonly Wave[] waves = new Wave[100];

        private Wave currentWave;
        public static int CurrentWaveIndex { get; private set; } = 0;

        public static event Action WaveStarted;
        public static event Action WaveFinished;

        public static event Action AllWavesFinished;

        public int CurrentWaveEnemiesDied => spawnManager.EnemiesDied - enemiesDiedBeforeWave;
        private int enemiesDiedBeforeWave;

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
            switch (currentWave.Type)
            {
                case WaveType.EnemyAmount:
                    if (currentWave.EnemyAmount <= 0 && spawnManager.EnemiesOnScene > 0)
                    {
                        waveUIInfo.text = spawnManager.EnemiesOnScene.ToString();
                    }
                    else if (currentWave.EnemyAmount > 0 || spawnManager.EnemiesOnScene > 0)
                    {
                        waveUIInfo.text = (currentWave.StartEnemyAmount - CurrentWaveEnemiesDied).ToString();
                    }
                    else
                    {
                        waveUIInfo.text = string.Empty;
                    }
                    break;

                case WaveType.Duration:
                    if (currentWave.DurationSec > 0)
                    {
                        waveUIInfo.text = Utilities.Watch((int)currentWave.DurationSec);
                    }
                    else
                    {
                        waveUIInfo.text = string.Empty;
                    }
                    break;
            }
        }

        private void WavesConstruct()
        {
            for (int i = 0; i < waves.Length; i++)
            {
                WaveType waveType = (WaveType)UnityEngine.Random.Range(0, 2);

                switch (waveType)
                {
                    case WaveType.EnemyAmount:
                        waves[i] = new Wave(
                            amount: 12 + i * 4,
                            spawnSpeed: 1.2f - i * 0.008f);
                        break;

                    case WaveType.Duration:
                        waves[i] = new Wave(
                            durationSec: 16 + i * 2,
                            spawnSpeed: 1.5f - i * 0.012f);
                        break;
                }

                if (i < minWaveForNight)
                    waves[i].TimesOfDay = TimesOfDay.Day;
            }
        }
        
        private IEnumerator WaveController()
        {
            switch (currentWave.Type)
            {
                case WaveType.EnemyAmount:
                    if (currentWave.EnemyAmount > 0)
                    {
                        yield return new WaitForSeconds(currentWave.SpawnSpeed);
                        currentWave.EnemyAmount--;

                        StartCoroutine(WaveController());
                    }
                    else
                    {
                        StartCoroutine(NextWaveOrRecheck());
                    }
                    break;

                case WaveType.Duration:
                    if (currentWave.DurationSec > 0)
                    {
                        yield return new WaitForSeconds(1);
                        currentWave.DurationSec--;

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
            if (spawnManager.EnemiesOnScene <= 0)
            {
                StartCoroutine(WaveTransition());
            }
            else //all enemies spawned, but not all died
            {
                spawnManager.Spawn = false;

                yield return new WaitForSeconds(1f);
                StartCoroutine(WaveController());
            }
        }

        private IEnumerator WaveTransition()
        {
            spawnManager.Spawn = false;
            yield return new WaitForSeconds(1);

            CurrentWaveIndex++;
            if (CurrentWaveIndex == waves.Length)
            {
                AllWavesFinished?.Invoke();
                yield break;
            }
            WaveFinished?.Invoke();
            
            lightManager.SetTimesOfDay(currentWave.TimesOfDay);

            yield return new WaitForSeconds(timeBtwWaves);

            StartWave();
        }

        private void StartWave()
        {
            currentWave = waves[CurrentWaveIndex];
            enemiesDiedBeforeWave += CurrentWaveEnemiesDied;

            spawnManager.Cooldown = currentWave.SpawnSpeed;

            spawnManager.Spawn = true;
            WaveStarted?.Invoke();

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
