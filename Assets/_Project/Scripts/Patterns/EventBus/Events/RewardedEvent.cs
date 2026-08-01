namespace EventBusLib
{
    public class RewardedEvent : IEvent
    {
        public int Reward { get; }

        public RewardedEvent(int reward)
        {
            Reward = reward;
        }
    }
}
