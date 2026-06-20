using Audio;
using EventBusLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PausePanel : MonoBehaviour, IIntermediatePanel
    {
        [SerializeField] private MusicManager _musicManager;
        [Space(10)]
        [SerializeField] private string _gameplaySceneName = "Gameplay";
        [SerializeField] private string _mainMenuSceneName = "MainMenu";

        public void Pause(bool value)
        {
            if (value)
            {
                Time.timeScale = 0f;
                _musicManager.Pause();
            }
            else
            {
                Time.timeScale = 1f;
                _musicManager.UnPause();
            }
        }

        public void Restart()
        {
            EventBus.Invoke(new GameExitEvent());
            SceneManager.LoadScene(_gameplaySceneName);
        }

        public void GoMainMenu()
        {
            EventBus.Invoke(new GameExitEvent());
            SceneManager.LoadScene(_mainMenuSceneName);
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}
