using TMPro;
using UnityEngine;

namespace WavesLib
{
    public abstract class WavePreset : MonoBehaviour
    {
        [SerializeField] protected float _min;
        [SerializeField] protected float _step;
        [Space(10)]
        [SerializeField] protected float _spawnSpeed;
        [SerializeField] protected float _spawnStep;
        [Space(10)]
        [SerializeField] protected Spawner<IEnemy> _spawner;
        [SerializeField] protected TextMeshProUGUI _text;

        public abstract Wave Construct(int boost);
    }
}
