using EventBusLib;
using LightLib;
using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WavesLib
{
    public abstract class Wave
    {
        public bool IsUsing = true;

        public TimesOfDay TimesOfDay = TimesOfDay.Day;
        public abstract IWaveState State { get; }

        public readonly float SpawnSpeed;

        public readonly Spawner<IEnemy> Spawner;
        public readonly TextMeshProUGUI Text;

        public int DestroyedObjects;

        public Wave(Spawner<IEnemy> spawner, TextMeshProUGUI text, float spawnSpeed, float nightChance)
        {
            float rndTimesOfDay = Random.Range(0, 1f);
            if (rndTimesOfDay <= nightChance)
                TimesOfDay = TimesOfDay.Night;
            
            Spawner = spawner;
            Text = text;

            SpawnSpeed = spawnSpeed;

            Spawner.Removed += OnRemoved;

            EventBus.Subscribe<WaveFinishedEvent>(UnSubscribe);
            EventBus.Subscribe<GameExitEvent>(UnSubscribe);
        }
        private void UnSubscribe(IEvent e)
        {
            EventBus.Unsubscribe<WaveFinishedEvent>(UnSubscribe);
            EventBus.Unsubscribe<GameExitEvent>(UnSubscribe);

            Spawner.Removed -= OnRemoved;
        }

        private void OnRemoved()
        {
            DestroyedObjects++;
        }
    }
}
