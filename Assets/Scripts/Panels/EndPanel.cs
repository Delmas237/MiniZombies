using EnemyLib;
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

        [SerializeField] private PlayerContainer _player;

        private void Start()
        {
            _player.HealthController.Died += Open;
            EnemyWaveManager.AllWavesFinished += Open;
        }
        private void OnDestroy()
        {
            _player.HealthController.Died -= Open;
            EnemyWaveManager.AllWavesFinished -= Open;
        }

        private void Open()
        {
            StartCoroutine(OpenCor());
        }
        private IEnumerator OpenCor()
        {
            yield return new WaitForSeconds(_timeToOpen);

            _endPanel.SetActive(true);
            int moneyWon = PlayerRewardsManager.GlobalCoinsWon();

            _wavesCompleted.text = $"You completed {EnemyWaveManager.CurrentWaveIndex} waves!";
            _moneyPlus.text = moneyWon.ToString();

            PlayerPrefs.SetInt("MaxWave", EnemyWaveManager.CurrentWaveIndex);
            Bank.Coins += moneyWon;
            Bank.Save();
        }

        public void Restart()
        {
            SceneManager.LoadScene("Game");
        }

        public void GoLobby()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
