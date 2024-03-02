namespace Schedoo.Server.Helpers;

public static class Extensions
{
    public static (DateTime Monday, DateTime Friday) GetMondayAndFriday()
    {
        DateTime today = DateTime.Today;
        DayOfWeek currentDayOfWeek = today.DayOfWeek;

        int daysToMonday = (int)DayOfWeek.Monday - (int)currentDayOfWeek;

        // If today is already Monday, set daysToMonday to 0
        if (daysToMonday > 0)
        {
            daysToMonday -= 7;
        }

        DateTime mondayOfWeek = today.AddDays(daysToMonday).Date;
        DateTime fridayOfWeek = mondayOfWeek.AddDays(4).Date;

        return (mondayOfWeek, fridayOfWeek);
    }
        
    public static DateTime GetDateOfWeekDay(DateTime currentDate, string dayOfWeek)
    {
        if (!Enum.TryParse<DayOfWeek>(dayOfWeek, true, out DayOfWeek targetDayOfWeek))
        {
            throw new ArgumentException("Invalid day of the week.", nameof(dayOfWeek));
        }

        DayOfWeek currentDayOfWeek = currentDate.DayOfWeek;

        int daysToTargetDay = (int)targetDayOfWeek - (int)currentDayOfWeek;
        DateTime targetDate = currentDate.AddDays(daysToTargetDay);

        return targetDate;
    }
    
    public static DateTime GetDateOfWeekDay(DateTime currentDate, DayOfWeek dayOfWeek)
    {
        DateTime today = currentDate;
        int daysUntilTarget = (int)dayOfWeek - (int)today.DayOfWeek;

        // If today is the target day, return today; otherwise, return the next occurrence.
        return today.AddDays(daysUntilTarget).Date;
    }

    public static WeekType ToWeekType(this string weekTypeStr)
    {
        var weekType = (WeekType)Enum.Parse(typeof(WeekType), weekTypeStr, true);

        return weekType;
    }

    public static DayOfWeek ToDayOfWeek(this string dayOfWeekStr)
    {
        var dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeekStr, true);

        return dayOfWeek;
    }
}