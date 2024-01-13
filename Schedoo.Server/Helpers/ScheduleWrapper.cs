using Schedoo.Server.Models;

namespace Schedoo.Server.Helpers
{
    public class ScheduleWrapper
    {
        public class ScheduleCat
        {
            public List<Schedule> OddWeekClasses { get; set; }
            public List<Schedule> EvenWeekClasses { get; set; }

            public ScheduleCat(List<Schedule> oddWeekClasses, List<Schedule> evenWeekClasses)
            {
                OddWeekClasses = oddWeekClasses;
                EvenWeekClasses = evenWeekClasses;
            }
        }

        public ScheduleCat ScheduleAll { get; set; }
        public List<string> Days { get; set; }
        public SortedSet<TimeSlot> TimeSlots { get; set; }
    }
}
