using TMPro;

namespace WavesLib
{
    public class WaveAmount : Wave
    {
        private readonly IWaveState _state;
        public override IWaveState State => _state;

        public readonly int StartEnemyAmount;
        public int EnemyAmount;

        public WaveAmount(Spawner<IEnemy> spawner, TextMeshProUGUI text, float spawnSpeed, int amount, float nightChance) : 
            base(spawner, text, spawnSpeed, nightChance)
        {
            EnemyAmount = amount;
            StartEnemyAmount = amount;

            _state = new WaveAmountState(this);
        }
    }
}
