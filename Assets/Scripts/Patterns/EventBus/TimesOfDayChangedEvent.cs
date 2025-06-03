using LightLib;

namespace EventBusLib
{
    public class TimesOfDayChangedEvent : IEvent
    {
        public readonly TimesOfDay TimesOfDay;

        public TimesOfDayChangedEvent(TimesOfDay timesOfDay)
        {
            TimesOfDay = timesOfDay;
        }
    }
}
