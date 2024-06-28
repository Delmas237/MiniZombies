using EventBusLib;
using LightLib;
using System;
using TMPro;
using Random = UnityEngine.Random;

namespace WavesLib
{
    public abstract class Wave
    {
        public bool IsUsing = true;

        public TimesOfDay TimesOfDay;
        public abstract IWaveState State { get; }

        public readonly float SpawnSpeed;

        public readonly Spawner<IEnemy> Spawner;
        public readonly TextMeshProUGUI Text;

        public int DestroyedObjects;

        public Wave(Spawner<IEnemy> spawner, TextMeshProUGUI text, float spawnSpeed)
        {
            TimesOfDay = (TimesOfDay)Random.Range(0, Enum.GetNames(typeof(TimesOfDay)).Length);
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
