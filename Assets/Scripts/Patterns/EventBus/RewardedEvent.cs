namespace EventBusLib
{
    public class RewardedEvent : IEvent
    {
        public readonly int Reward;

        public RewardedEvent(int reward)
        {
            Reward = reward;
        }
    }
}
