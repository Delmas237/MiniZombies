using LightLib;

namespace EventBusLib
{
    public class TimesOfDayChangedEvent : IEvent
    {
        private TimesOfDay _timesOfDay;
        public TimesOfDay TimesOfDay => _timesOfDay;

        public TimesOfDayChangedEvent(TimesOfDay timesOfDay)
        {
            _timesOfDay = timesOfDay;
        }
    }
}
