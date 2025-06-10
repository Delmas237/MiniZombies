using EventBusLib;
using UnityEngine;

namespace PlayerLib
{
    public class PlayerRewardsManager : MonoBehaviour
    {
        [SerializeField] private PlayerContainer _player;
        [SerializeField] private string _dataPath = "Data/RewardData";

        private float _globalWaveReward;
        private float _completedAllWavesReward;
        private float _localKillReward;

        private void Start()
        {
            EventBus.Subscribe<WaveFinishedEvent>(GetLocalReward);
            EventBus.Subscribe<GameOverEvent>(GetGlobalReward);
            EventBus.Subscribe<AllWavesFinishedEvent>(GetGlobalReward);

            RewardData reward = Resources.Load<RewardData>(_dataPath);

            _localKillReward = reward.LocalKillReward;
            _globalWaveReward = reward.GlobalWaveReward;
            _completedAllWavesReward = reward.CompletedAllWavesReward;
        }

        private void GetLocalReward(WaveFinishedEvent waveFinishedEvent)
        {
            float reward = waveFinishedEvent.Wave.DestroyedObjects * _localKillReward;

            int intReward = Mathf.RoundToInt(reward);
            _player.CurrencyModule.Add(intReward);
        }

        private void GetGlobalReward(GameOverEvent gameOverEvent) => GetGlobalReward(gameOverEvent.CompletedWaves);
        private void GetGlobalReward(AllWavesFinishedEvent allWavesFinishedEvent) => GetGlobalReward(allWavesFinishedEvent.Number, true);
        private void GetGlobalReward(int wave, bool allWavesFinished = false)
        {
            float reward = (wave * _globalWaveReward) + (Mathf.Pow(wave, 2) / 10f);

            if (allWavesFinished)
                reward += _completedAllWavesReward;

            int intReward = Mathf.RoundToInt(reward);
            Bank.Add(intReward);

            EventBus.Invoke(new RewardedEvent(intReward));
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<WaveFinishedEvent>(GetLocalReward);
            EventBus.Unsubscribe<GameOverEvent>(GetGlobalReward);
            EventBus.Unsubscribe<AllWavesFinishedEvent>(GetGlobalReward);
        }
    }
}
