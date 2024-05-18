using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Panels
{
    public class PausePanel : MonoBehaviour, IIntermediatePanel
    {
        [SerializeField] private MusicManager _musicManager;

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
            SceneManager.LoadScene("Game");
        }

        public void GoLobby()
        {
            SceneManager.LoadScene("Lobby");
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}
