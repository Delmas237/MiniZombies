using TMPro;

namespace WavesLib
{
    public class WaveTime : Wave
    {
        private readonly IWaveState _state;
        public override IWaveState State => _state;

        public float Time;

        public WaveTime(Spawner<IEnemy> spawner, TextMeshProUGUI text, float spawnSpeed, float time) : base(spawner, text, spawnSpeed)
        {
            Time = time;

            _state = new WaveTimeState(this);
        }
    }
}
