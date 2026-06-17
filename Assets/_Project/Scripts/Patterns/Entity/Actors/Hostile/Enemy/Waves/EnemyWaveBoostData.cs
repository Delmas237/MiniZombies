using System;
using UnityEngine;

namespace EnemyLib
{
    [Serializable]
    public class EnemyWaveBoostData
    {
        [SerializeField] private float _hpPercent = 0.03f;
        [SerializeField] private float _waveMultiplierSpeed = 0.01f;
        
        public float HpPercent => _hpPercent;
        public float WaveMultiplierSpeed => _waveMultiplierSpeed;
    }
}
