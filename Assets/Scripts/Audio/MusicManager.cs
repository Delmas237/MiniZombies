using EnemyLib;
using PlayerLib;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup mixer;

        [SerializeField] private AudioSource calmMusic;
        [SerializeField] private AudioSource battleMusic;

        [SerializeField] private SoundSettings soundSettings;
        [SerializeField] private PlayerContainer player;

        private MusicType state = MusicType.Calm;

        private void Start()
        {
            EnemyWaveManager.WaveStarted += Change;
            EnemyWaveManager.WaveFinished += Change;
            player.HealthController.Died += Stop;
        }
        private void OnDestroy()
        {
            EnemyWaveManager.WaveStarted -= Change;
            EnemyWaveManager.WaveFinished -= Change;
            player.HealthController.Died -= Stop;
        }

        private void Play(MusicType musicType)
        {
            Pause();

            switch (musicType)
            {
                case MusicType.Battle:
                    battleMusic.Play();
                    break;
                case MusicType.Calm:
                    calmMusic.Play();
                    break;
            }
        }

        private void Stop()
        {
            battleMusic.Stop();
            calmMusic.Stop();
        }

        public void Pause()
        {
            battleMusic.Pause();
            calmMusic.Pause();
        }

        public void UnPause()
        {
            switch (state)
            {
                case MusicType.Battle:
                    battleMusic.UnPause();
                    break;
                case MusicType.Calm:
                    calmMusic.UnPause();
                    break;
            }
        }

        private void Change()
        {
            float timeBtwSteps = 0f;

            switch (state)
            {
                case MusicType.Battle:
                    timeBtwSteps = 0.03f;
                    state = MusicType.Calm;
                    break;
                case MusicType.Calm:
                    timeBtwSteps = 0.01f;
                    state = MusicType.Battle;
                    break;
            }

            StartCoroutine(VolumeTransition(-20, 1, timeBtwSteps, state));
        }

        private IEnumerator VolumeTransition(float value, float step, float timeBtwSteps, MusicType musicType)
        {
            string musicVolume = "MusicVolume";
            mixer.audioMixer.GetFloat(musicVolume, out float volume);

            if (value <= volume)
            {
                while (value <= volume)
                {
                    yield return new WaitForSeconds(timeBtwSteps);
                    volume -= step;
                    mixer.audioMixer.SetFloat(musicVolume, volume);
                }
                StartCoroutine(VolumeTransition(soundSettings.MusicVolume, step, timeBtwSteps, musicType));
            }
            else
            {
                while (value >= volume)
                {
                    yield return new WaitForSeconds(timeBtwSteps);
                    volume += step;
                    mixer.audioMixer.SetFloat(musicVolume, volume);
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
