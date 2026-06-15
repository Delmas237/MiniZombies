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
        [SerializeField] private string _completedString = "You completed {0} waves!";
        [Space(10)]
        [SerializeField] private GameObject _endPanel;
        [SerializeField] private TextMeshProUGUI _wavesCompleted;
        [SerializeField] private TextMeshProUGUI _moneyPlus;
        [Space(10)]
        [SerializeField] private string _gameplaySceneName = "Gameplay";
        [SerializeField] private string _mainMenuSceneName = "MainMenu";

        private int _waveNumber;
        private string _maxWaveId = "MaxWave";

        private void Start()
        {
            EventBus.Subscribe<AllWavesFinishedEvent>(SetWaveNumber);
            EventBus.Subscribe<GameOverEvent>(SetWaveNumber);
            EventBus.Subscribe<RewardedEvent>(Open);
        }
        private void SetWaveNumber(GameOverEvent gameOverEvent) => _waveNumber = gameOverEvent.CompletedWaves;
        private void SetWaveNumber(AllWavesFinishedEvent allWavesFinishedEvent) => _waveNumber = allWavesFinishedEvent.Number;

        private void Open(RewardedEvent rewardedEvent) => StartCoroutine(OpenCor(rewardedEvent.Reward));
        private IEnumerator OpenCor(int reward)
        {
            yield return new WaitForSeconds(_timeToOpen);

            _endPanel.SetActive(true);

            _wavesCompleted.text = string.Format(_completedString, _waveNumber);
            _moneyPlus.text = reward.ToString();

            if (PlayerPrefs.GetInt(_maxWaveId) < _waveNumber)
                PlayerPrefs.SetInt(_maxWaveId, _waveNumber);
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
            EventBus.Unsubscribe<AllWavesFinishedEvent>(SetWaveNumber);
            EventBus.Unsubscribe<GameOverEvent>(SetWaveNumber);
            EventBus.Unsubscribe<RewardedEvent>(Open);
        }
    }
}
