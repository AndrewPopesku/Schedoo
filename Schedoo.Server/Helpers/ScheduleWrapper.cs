using Schedoo.Server.Models;
using System.Collections.Immutable;

namespace Schedoo.Server.Helpers
{
    public class ScheduleWrapper
    {
        public class ScheduleCat
        {
            public IEnumerable<Schedule> OddWeekClasses { get; set; }
            public IEnumerable<Schedule> EvenWeekClasses { get; set; }

            public ScheduleCat(IEnumerable<Schedule> oddWeekClasses, IEnumerable<Schedule> evenWeekClasses)
            {
                OddWeekClasses = oddWeekClasses;
                EvenWeekClasses = evenWeekClasses;
            }
        }

        public ScheduleCat ScheduleAll { get; set; }
        public IEnumerable<string> Days { get; set; }
        public ImmutableSortedSet<TimeSlot> TimeSlots { get; set; }
    }
}
