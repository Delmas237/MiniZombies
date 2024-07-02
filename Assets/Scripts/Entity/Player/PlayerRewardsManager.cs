using EventBusLib;
using System;
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
            return wave switch
            {
                <= 5 => wave * 12,
                <= 10 => wave * 14,
                <= 20 => wave * 16,
                <= 35 => wave * 18,
                <= 60 => wave * 20,
                < 100 => wave * 22,
                >= 100 => wave * 22 + 1000,
            };
        }
    }
}
