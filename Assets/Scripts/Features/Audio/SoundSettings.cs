using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class SoundSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _mixer;

        [SerializeField] private Slider _soundsSlider;
        [SerializeField] private Slider _musicSlider;

        public float SoundsVolume => _soundsSlider.value;
        public float MusicVolume => _musicSlider.value;

        private void Start()
        {
            Load();

            _soundsSlider.onValueChanged.AddListener(Save);
            _musicSlider.onValueChanged.AddListener(Save);
            _soundsSlider.onValueChanged.AddListener(UpdateVolume);
            _musicSlider.onValueChanged.AddListener(UpdateVolume);
        }
        private void Save(float value) => Save();
        private void UpdateVolume(float value) => UpdateVolume();

        private void Load()
        {
            _soundsSlider.value = PlayerPrefs.GetFloat("SoundsVolume");
            _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

            UpdateVolume();
        }
        private void Save()
        {
            PlayerPrefs.SetFloat("SoundsVolume", _soundsSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
        }

        private void UpdateVolume()
        {
            SetVolume("SoundsVolume", _soundsSlider.value);
            SetVolume("MusicVolume", _musicSlider.value);
        }
        private void SetVolume(string nameVolume, float value)
        {
            if (value <= -40)
            {
                _mixer.audioMixer.SetFloat(nameVolume, -80);
                return;
            }

            _mixer.audioMixer.SetFloat(nameVolume, value);
        }
    }
}
