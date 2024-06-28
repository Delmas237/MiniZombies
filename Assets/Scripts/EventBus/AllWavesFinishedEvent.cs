using WavesLib;

namespace EventBusLib
{
    public class AllWavesFinishedEvent : IEvent 
    {
        public readonly Wave Wave;
        public readonly int Number;

        public AllWavesFinishedEvent(Wave wave, int number)
        {
            Wave = wave;
            Number = number;
        }
    }
}
