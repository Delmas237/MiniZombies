using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private TextMeshProUGUI _maxWaveText;
        [Space(10)]
        [SerializeField] private string _gameplaySceneName = "Gameplay";

        private string _maxWaveId = "MaxWave";

        private void Awake()
        {
            _maxWaveText.text = PlayerPrefs.GetInt(_maxWaveId).ToString();

            UpdateText(Bank.Coins);
            Bank.CoinsChanged += UpdateText;
        }

        private void UpdateText(int amount)
        {
            _coinsText.text = amount.ToString();
        }

        public void Play() => SceneManager.LoadScene(_gameplaySceneName);

        private void OnDestroy()
        {
            Bank.CoinsChanged -= UpdateText;
        }
    }
}
