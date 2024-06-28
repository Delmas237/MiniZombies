using EventBusLib;
using PlayerLib;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Panels
{
    public class EndPanel : MonoBehaviour, IIntermediatePanel
    {
        [SerializeField] private float _timeToOpen;
        [Space(10)]
        [SerializeField] private GameObject _endPanel;
        [SerializeField] private TextMeshProUGUI _wavesCompleted;
        [SerializeField] private TextMeshProUGUI _moneyPlus;

        private void Start()
        {
            EventBus.Subscribe<GameOverEvent>(Open);
            EventBus.Subscribe<AllWavesFinishedEvent>(Open);
        }
        private void OnDestroy()
        {
            EventBus.Unsubscribe<GameOverEvent>(Open);
            EventBus.Unsubscribe<AllWavesFinishedEvent>(Open);
        }

        private void Open(GameOverEvent gameOverEvent) => Open(gameOverEvent.Wave);
        private void Open(AllWavesFinishedEvent allWavesFinishedEvent) => Open(allWavesFinishedEvent.Number);
        private void Open(int wave) => StartCoroutine(OpenCor(wave));
        private IEnumerator OpenCor(int waveNumber)
        {
            yield return new WaitForSeconds(_timeToOpen);

            _endPanel.SetActive(true);
            int moneyWon = PlayerRewardsManager.GetGlobalCoinsReward(waveNumber);

            _wavesCompleted.text = $"You completed {waveNumber} waves!";
            _moneyPlus.text = moneyWon.ToString();

            if (PlayerPrefs.GetInt("MaxWave") < waveNumber)
                PlayerPrefs.SetInt("MaxWave", waveNumber);

            Bank.Coins += moneyWon;
        }

        public void Restart()
        {
            EventBus.Invoke(new GameExitEvent());
            SceneManager.LoadScene("Game");
        }

        public void GoLobby()
        {
            EventBus.Invoke(new GameExitEvent());
            SceneManager.LoadScene("Lobby");
        }
    }
}
