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
        [SerializeField] private float timeToOpen;
        [Space(10)]
        [SerializeField] private GameObject endPanel;
        [SerializeField] private TextMeshProUGUI wavesCompleted;
        [SerializeField] private TextMeshProUGUI moneyPlus;

        [SerializeField] private Player player;

        private void Start()
        {
            player.HealthController.Died += Open;
            EnemyWaveManager.AllWavesFinished += Open;
        }
        private void OnDestroy()
        {
            player.HealthController.Died -= Open;
            EnemyWaveManager.AllWavesFinished -= Open;
        }

        private void Open()
        {
            StartCoroutine(OpenCor());
        }
        private IEnumerator OpenCor()
        {
            yield return new WaitForSeconds(timeToOpen);

            endPanel.SetActive(true);
            int moneyWon = PlayerRewardsManager.GlobalCoinsWon();

            wavesCompleted.text = $"You completed {EnemyWaveManager.CurrentWaveIndex} waves!";
            moneyPlus.text = moneyWon.ToString();

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
