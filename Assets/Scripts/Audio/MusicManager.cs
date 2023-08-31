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

        [SerializeField] private AudioSource battleMusic;
        [SerializeField] private AudioSource betweenWavesMusic;

        [SerializeField] private SoundSettings soundSettings;
        [SerializeField] private Player player;

        private MusicType currentMusic = MusicType.Battle;
        private bool isFirstWave = true;

        private void Start()
        {
            EnemyWaveManager.WaveFinished += IsFirstWaveFalse;
            EnemyWaveManager.WaveStarted += Change;
            EnemyWaveManager.WaveFinished += Change;
            player.HealthController.Died += Stop;
        }
        private void OnDestroy()
        {
            EnemyWaveManager.WaveFinished -= IsFirstWaveFalse;
            EnemyWaveManager.WaveStarted -= Change;
            EnemyWaveManager.WaveFinished -= Change;
            player.HealthController.Died -= Stop;
        }

        private void IsFirstWaveFalse() => isFirstWave = false;

        private void Play(MusicType musicType)
        {
            Pause();

            switch (musicType)
            {
                case MusicType.Battle:
                    battleMusic.Play();
                    break;
                case MusicType.BetweenWaves:
                    betweenWavesMusic.Play();
                    break;
            }
        }

        private void Stop()
        {
            battleMusic.Stop();
            betweenWavesMusic.Stop();
        }

        public void Pause()
        {
            battleMusic.Pause();
            betweenWavesMusic.Pause();
        }

        public void UnPause()
        {
            switch (currentMusic)
            {
                case MusicType.Battle:
                    battleMusic.UnPause();
                    break;
                case MusicType.BetweenWaves:
                    betweenWavesMusic.UnPause();
                    break;
            }
        }

        private void Change()
        {
            if (isFirstWave == false)
            {
                switch (currentMusic)
                {
                    case MusicType.Battle:
                        StartCoroutine(VolumeTransition(-20, 1, 0.03f, MusicType.BetweenWaves));
                        currentMusic = MusicType.BetweenWaves;
                        break;
                    case MusicType.BetweenWaves:
                        StartCoroutine(VolumeTransition(-20, 1, 0.01f, MusicType.Battle));
                        currentMusic = MusicType.Battle;
                        break;
                }
            }
        }

        private IEnumerator VolumeTransition(float value, float step, float timeBtwSteps, MusicType musicType)
        {
            mixer.audioMixer.GetFloat("MusicVolume", out float volume);

            if (value <= volume)
            {
                while (value <= volume)
                {
                    yield return new WaitForSeconds(timeBtwSteps);
                    volume -= step;
                    mixer.audioMixer.SetFloat("MusicVolume", volume);
                }
                StartCoroutine(VolumeTransition(soundSettings.MusicVolume, step, timeBtwSteps, musicType));
            }
            else
            {
                while (value >= volume)
                {
                    yield return new WaitForSeconds(timeBtwSteps);
                    volume += step;
                    mixer.audioMixer.SetFloat("MusicVolume", volume);
                }
            }

            Play(musicType);
        }

        private enum MusicType
        {
            Battle,
            BetweenWaves
        }
    }
}
