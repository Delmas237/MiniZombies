using TMPro;

namespace WavesLib
{
    public class WaveAmount : Wave
    {
        public override IWaveState State { get; }
        public int StartEnemyAmount { get; }
        public int EnemyAmount { get; set; }

        public WaveAmount(Spawner<IEnemy> spawner, TextMeshProUGUI text, float spawnSpeed, int amount, float nightChance) : 
            base(spawner, text, spawnSpeed, nightChance)
        {
            EnemyAmount = amount;
            StartEnemyAmount = amount;

            State = new WaveAmountState(this);
        }
    }
}
