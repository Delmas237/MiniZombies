using WavesLib;

namespace EventBusLib
{
    public class WaveFinishedEvent : IEvent
    {
        public Wave Wave { get; }
        public int Number { get; }

        public WaveFinishedEvent(Wave wave, int number)
        {
            Wave = wave;
            Number = number;
        }
    }
}
