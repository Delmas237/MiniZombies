namespace EventBusLib
{
    public class GameOverEvent : IEvent 
    {
        public readonly int Wave;

        public GameOverEvent(int wave)
        {
            Wave = wave;
        }
    }
}
