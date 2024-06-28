namespace WavesLib
{
    public class WaveTimePreset : WavePreset
    {
        public override Wave Construct(int boost)
        {
            Wave wave = new WaveTime(_spawner, _text,
                spawnSpeed: _spawnSpeed - boost * _spawnStep,
                time: _min + boost * _step);

            return wave;
        }
    }
}
