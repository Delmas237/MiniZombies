using WavesLib;

namespace EventBusLib
{
    public class WaveStartedEvent : IEvent 
    {
        public Wave Wave { get; }
        public int Number { get; }

        public WaveStartedEvent(Wave wave, int number) 
        {
            Wave = wave;
            Number = number;
        }
    }
}
