using EventBusLib;
using UnityEngine;

namespace PlayerLib
{
    public class PlayerRewardsManager : MonoBehaviour
    {
        [SerializeField] private float _killReward = 6;
        [SerializeField] private PlayerContainer _player;

        private void Start()
        {
            EventBus.Subscribe<WaveFinishedEvent>(GetLocalCoinsReward);
        }
        private void OnDestroy()
        {
            EventBus.Unsubscribe<WaveFinishedEvent>(GetLocalCoinsReward);
        }

        private void GetLocalCoinsReward(WaveFinishedEvent waveFinishedEvent)
        {
            _player.CurrencyController.Add(Mathf.RoundToInt(waveFinishedEvent.Wave.DestroyedObjects * _killReward));
        }

        public static int GetGlobalCoinsReward(int wave)
        {
            if (wave <= 5)
                return wave * 10;

            else if (wave <= 10)
                return wave * 12;

            else if (wave <= 20)
                return wave * 14;

            else if (wave <= 35)
                return wave * 16;

            else if (wave <= 60)
                return wave * 18;

            else if (wave < 100)
                return wave * 20;

            else if (wave >= 100)
                return wave * 20 + 1000;

            Debug.LogError("Number of waves out of range");
            return 0;
        }
    }
}
