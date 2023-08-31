using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class SoundSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup mixer;

        [SerializeField] private Slider soundsSlider;
        [SerializeField] private Slider musicSlider;

        public float SoundsVolume => soundsSlider.value;
        public float MusicVolume => musicSlider.value;

        private void Start()
        {
            Load();

            soundsSlider.onValueChanged.AddListener(Save);
            musicSlider.onValueChanged.AddListener(Save);
            soundsSlider.onValueChanged.AddListener(UpdateVolume);
            musicSlider.onValueChanged.AddListener(UpdateVolume);
        }
        private void Save(float value) => Save();
        private void UpdateVolume(float value) => UpdateVolume();

        private void Load()
        {
            soundsSlider.value = PlayerPrefs.GetFloat("SoundsVolume");
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

            UpdateVolume();
        }
        private void Save()
        {
            PlayerPrefs.SetFloat("SoundsVolume", soundsSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        }

        private void UpdateVolume()
        {
            SetVolume("SoundsVolume", soundsSlider.value);
            SetVolume("MusicVolume", musicSlider.value);
        }
        private void SetVolume(string nameVolume, float value)
        {
            if (value <= -40)
            {
                mixer.audioMixer.SetFloat(nameVolume, -80);
                return;
            }

            mixer.audioMixer.SetFloat(nameVolume, value);
        }
    }
}
