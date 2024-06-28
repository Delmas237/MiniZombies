using EventBusLib;
using System;
using System.Collections;
using UnityEngine;

namespace WavesLib
{
    public class WaveTimeState : IWaveState
    {
        private readonly WaveTime _wave;

        private Coroutine _control;
        private Coroutine _waitTransition;

        public WaveTimeState(WaveTime wave)
        {
            _wave = wave;

            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(IEvent e)
        {
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);

            if (_control != null)
                CoroutineHelper.StopRoutine(_control);

            if (_waitTransition != null)
                CoroutineHelper.StopRoutine(_waitTransition);
        }

        public IEnumerator Control(Func<IEnumerator> waitTransition)
        {
            if (_wave.Time > 0)
            {
                yield return new WaitForSeconds(1);
                _wave.Time--;

                _control = CoroutineHelper.StartRoutine(Control(waitTransition));
            }
            else
            {
                _wave.IsUsing = false;
                _wave.Spawner.IsSpawn = false;
                _waitTransition = CoroutineHelper.StartRoutine(waitTransition.Invoke());
            }
            UpdateUIInfo();
        }

        public void UpdateUIInfo()
        {
            if (_wave.Time > 0)
                _wave.Text.text = Utilities.Watch(Mathf.RoundToInt(_wave.Time));
            else
                _wave.Text.text = string.Empty;
        }
    }
}
