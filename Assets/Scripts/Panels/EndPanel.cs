using EventBusLib;
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

        private int _waveNumber;

        private void Start()
        {
            EventBus.Subscribe<RewardedEvent>(Open);
            EventBus.Subscribe<AllWavesFinishedEvent>(Open);
            EventBus.Subscribe<GameOverEvent>(SetWaveNumber);
        }
        private void OnDestroy()
        {
            EventBus.Unsubscribe<RewardedEvent>(Open);
            EventBus.Unsubscribe<AllWavesFinishedEvent>(Open);
            EventBus.Unsubscribe<GameOverEvent>(SetWaveNumber);
        }
        private void SetWaveNumber(GameOverEvent gameOverEvent) => _waveNumber = gameOverEvent.CompletedWaves;

        private void Open(RewardedEvent rewardedEvent) => StartCoroutine(OpenCor(rewardedEvent.Reward));
        private void Open(AllWavesFinishedEvent allWavesFinishedEvent) => StartCoroutine(OpenCor(allWavesFinishedEvent.Number));
        private IEnumerator OpenCor(int reward)
        {
            yield return new WaitForSeconds(_timeToOpen);

            _endPanel.SetActive(true);

            _wavesCompleted.text = $"You completed {_waveNumber} waves!";
            _moneyPlus.text = reward.ToString();

            if (PlayerPrefs.GetInt("MaxWave") < _waveNumber)
                PlayerPrefs.SetInt("MaxWave", _waveNumber);
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
