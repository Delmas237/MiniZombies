using WavesLib;

namespace EventBusLib
{
    public class WaveFinishedEvent : IEvent
    {
        public readonly Wave Wave;
        public readonly int Number;

        public WaveFinishedEvent(Wave wave, int number)
        {
            Wave = wave;
            Number = number;
        }
    }
}
