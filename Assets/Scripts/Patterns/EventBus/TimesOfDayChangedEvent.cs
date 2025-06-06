using LightLib;

namespace EventBusLib
{
    public class TimesOfDayChangedEvent : IEvent
    {
        public TimesOfDay TimesOfDay { get; }

        public TimesOfDayChangedEvent(TimesOfDay timesOfDay)
        {
            TimesOfDay = timesOfDay;
        }
    }
}
