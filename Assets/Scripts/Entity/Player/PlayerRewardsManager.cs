using EnemyLib;
using UnityEngine;

namespace PlayerLib
{
    public class PlayerRewardsManager : MonoBehaviour
    {
        [SerializeField] private PlayerContainer player;
        [SerializeField] private EnemyWaveManager enemyWaveManager;

        private void Start()
        {
            EnemyWaveManager.WaveFinished += LocalCoinsWon;
        }
        private void OnDestroy()
        {
            EnemyWaveManager.WaveFinished -= LocalCoinsWon;
        }

        private void LocalCoinsWon() => player.CurrencyController.Coins += enemyWaveManager.CurrentWaveEnemiesDied * 5;

        public static int GlobalCoinsWon()
        {
            int numberOfWaves = EnemyWaveManager.CurrentWaveIndex;

            if (numberOfWaves <= 5)
                return numberOfWaves * 10;

            else if (numberOfWaves <= 10)
                return numberOfWaves * 12;

            else if (numberOfWaves <= 20)
                return numberOfWaves * 14;

            else if (numberOfWaves <= 35)
                return numberOfWaves * 16;

            else if (numberOfWaves <= 60)
                return numberOfWaves * 18;

            else if (numberOfWaves < 100)
                return numberOfWaves * 20;

            else if (numberOfWaves == 100)
                return numberOfWaves * 20 + 1000;

            Debug.LogError("Number of waves out of range");
            return 0;
        }
    }
}
