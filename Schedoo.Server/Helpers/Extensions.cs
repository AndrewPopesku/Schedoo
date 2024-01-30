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

        DateTime mondayOfWeek = today.AddDays(daysToMonday);
        DateTime fridayOfWeek = mondayOfWeek.AddDays(4);

        return (mondayOfWeek, fridayOfWeek);
    }
        
    public static DateTime GetDateOfWeekDay(string dayOfWeek)
    {
        if (!Enum.TryParse<DayOfWeek>(dayOfWeek, true, out DayOfWeek targetDayOfWeek))
        {
            throw new ArgumentException("Invalid day of the week.", nameof(dayOfWeek));
        }

        DateTime currentDate = DateTime.Today;
        DayOfWeek currentDayOfWeek = currentDate.DayOfWeek;

        int daysToTargetDay = ((int)targetDayOfWeek - (int)currentDayOfWeek + 7) % 7;
        DateTime targetDate = currentDate.AddDays(daysToTargetDay);

        return targetDate;
    }
}