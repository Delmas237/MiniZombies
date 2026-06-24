using Entity.Hostile;
using TMPro;

namespace Waves
{
    public class WaveTime : Wave
    {
        public override IWaveState State { get; }
        public float Time { get; set; }

        public WaveTime(Spawner<IHostile> spawner, TextMeshProUGUI text, float spawnSpeed, float time, float nightChance) : 
            base(spawner, text, spawnSpeed, nightChance)
        {
            Time = time;

            State = new WaveTimeState(this);
        }
    }
}
