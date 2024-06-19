using EventBusLib;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _mixer;

        [SerializeField] private AudioSource _calmMusic;
        [SerializeField] private AudioSource _battleMusic;

        [SerializeField] private SoundSettings _soundSettings;

        private MusicType _state = MusicType.Calm;

        private void Start()
        {
            EventBus.Subscribe<WaveStartedEvent>(Change);
            EventBus.Subscribe<WaveFinishedEvent>(Change);

            EventBus.Subscribe<GameOverEvent>(Stop);
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent gameExitEvent)
        {
            EventBus.Unsubscribe<WaveStartedEvent>(Change);
            EventBus.Unsubscribe<WaveFinishedEvent>(Change);

            EventBus.Unsubscribe<GameOverEvent>(Stop);
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
        }

        private void Play(MusicType musicType)
        {
            Pause();

            switch (musicType)
            {
                case MusicType.Battle:
                    _battleMusic.Play();
                    break;
                case MusicType.Calm:
                    _calmMusic.Play();
                    break;
            }
        }

        private void Stop(IEvent e) => Stop();
        private void Stop()
        {
            _battleMusic.Stop();
            _calmMusic.Stop();
        }

        public void Pause()
        {
            _battleMusic.Pause();
            _calmMusic.Pause();
        }

        public void UnPause()
        {
            switch (_state)
            {
                case MusicType.Battle:
                    _battleMusic.UnPause();
                    break;
                case MusicType.Calm:
                    _calmMusic.UnPause();
                    break;
            }
        }

        private void Change(IEvent e) => Change();
        private void Change()
        {
            float timeBtwSteps = 0f;

            switch (_state)
            {
                case MusicType.Battle:
                    timeBtwSteps = 0.03f;
                    _state = MusicType.Calm;
                    break;
                case MusicType.Calm:
                    timeBtwSteps = 0.01f;
                    _state = MusicType.Battle;
                    break;
            }

            StartCoroutine(VolumeTransition(-20, 1, timeBtwSteps, _state));
        }

        private IEnumerator VolumeTransition(float value, float step, float timeBtwSteps, MusicType musicType)
        {
            string musicVolume = "MusicVolume";
            _mixer.audioMixer.GetFloat(musicVolume, out float volume);

            if (value <= volume)
            {
                while (value <= volume)
                {
                    yield return new WaitForSeconds(timeBtwSteps);
                    volume -= step;
                    _mixer.audioMixer.SetFloat(musicVolume, volume);
                }
                StartCoroutine(VolumeTransition(_soundSettings.MusicVolume, step, timeBtwSteps, musicType));
            }
            else
            {
                while (value >= volume)
                {
                    yield return new WaitForSeconds(timeBtwSteps);
                    volume += step;
                    _mixer.audioMixer.SetFloat(musicVolume, volume);
                }
            }

            Play(musicType);
        }

        private enum MusicType
        {
            Calm,
            Battle
        }
    }
}
