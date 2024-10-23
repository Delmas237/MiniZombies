using WavesLib;

namespace EventBusLib
{
    public class WaveStartedEvent : IEvent 
    {
        public readonly Wave Wave;
        public readonly int Number;

        public WaveStartedEvent(Wave wave, int number) 
        {
            Wave = wave;
            Number = number;
        }
    }
}
