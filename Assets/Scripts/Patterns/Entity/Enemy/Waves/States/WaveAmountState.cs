using EventBusLib;
using System;
using System.Collections;
using UnityEngine;

namespace WavesLib
{
    public class WaveAmountState : IWaveState
    {
        private readonly WaveAmount _wave;

        private Func<IEnumerator> _waitTransition;
        private Coroutine _waitTransitionCor;

        public WaveAmountState(WaveAmount wave)
        {
            _wave = wave;

            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(IEvent e)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);

            if (_waitTransitionCor != null)
                CoroutineHelper.StopRoutine(_waitTransitionCor);
        }

        public IEnumerator Control(Func<IEnumerator> waitTransition)
        {
            _waitTransition = waitTransition;

            _wave.Spawner.Spawned += HandleSpawned;
            _wave.Spawner.Removed += HandleRemoved;

            yield break;
        }

        private void HandleSpawned(IEntity entity)
        {
            _wave.EnemyAmount--;

            if (_wave.EnemyAmount <= 0)
            {
                _wave.Spawner.Spawned -= HandleSpawned;
                _wave.Spawner.IsSpawn = false;
                _waitTransitionCor = CoroutineHelper.StartRoutine(_waitTransition.Invoke());
            }
            UpdateUIInfo();
        }
        private void HandleRemoved()
        {
            if (_wave.EnemyAmount <= 0 && Spawner<IEnemy>.ObjectsOnScene.Count <= 0)
            {
                _wave.IsUsing = false;
                _wave.Spawner.Removed -= HandleRemoved;
            }
            UpdateUIInfo();
        }

        public void UpdateUIInfo()
        {
            if (_wave.EnemyAmount <= 0 && Spawner<IEnemy>.ObjectsOnScene.Count > 0)
            {
                _wave.Text.text = Spawner<IEnemy>.ObjectsOnScene.Count.ToString();
            }
            else if (_wave.EnemyAmount > 0 || Spawner<IEnemy>.ObjectsOnScene.Count > 0)
            {
                _wave.Text.text = (_wave.StartEnemyAmount - _wave.DestroyedObjects).ToString();
            }
            else
            {
                _wave.Text.text = string.Empty;
            }
        }
    }
}
