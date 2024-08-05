using UnityEngine;

namespace WavesLib
{
    public class WaveAmountPreset : WavePreset
    {
        public override Wave Construct(int boost)
        {
            Wave wave = new WaveAmount(_spawner, _text,
                spawnSpeed: _spawnSpeed - boost * _spawnStep,
                amount: Mathf.RoundToInt(_min + boost * _step), _nightChance);

            return wave;
        }
    }
}
