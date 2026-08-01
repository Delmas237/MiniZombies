namespace EventBusLib
{
    public class GameOverEvent : IEvent 
    {
        public int CompletedWaves { get; }

        public GameOverEvent(int completedWaves)
        {
            CompletedWaves = completedWaves;
        }
    }
}
