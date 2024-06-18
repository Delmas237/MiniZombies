using LightLib;

namespace EventBusLib
{
    public class TimesOfDayChangedEvent
    {
        private TimesOfDay _timesOfDay;
        public TimesOfDay TimesOfDay => _timesOfDay;

        public TimesOfDayChangedEvent(TimesOfDay timesOfDay)
        {
            _timesOfDay = timesOfDay;
        }
    }
}
