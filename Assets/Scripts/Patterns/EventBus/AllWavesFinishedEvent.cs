using WavesLib;

namespace EventBusLib
{
    public class AllWavesFinishedEvent : IEvent 
    {
        public Wave Wave { get; }
        public int Number { get; }

        public AllWavesFinishedEvent(Wave wave, int number)
        {
            Wave = wave;
            Number = number;
        }
    }
}
