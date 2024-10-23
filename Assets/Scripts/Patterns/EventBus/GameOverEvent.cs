namespace EventBusLib
{
    public class GameOverEvent : IEvent 
    {
        public readonly int CompletedWaves;

        public GameOverEvent(int completedWaves)
        {
            CompletedWaves = completedWaves;
        }
    }
}
