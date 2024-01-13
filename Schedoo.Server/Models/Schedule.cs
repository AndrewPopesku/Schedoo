namespace Schedoo.Server.Models;

public class Schedule
{
    public int? Id { get; set; }
    public string WeekType { get; set; }
    public int TimeSlotId { get; set; }
    public string DayOfWeek { get; set; }
    public int ClassId { get; set; }
    public int GroupId { get; set; }

    public virtual TimeSlot TimeSlot { get; set; }
    public virtual Class Class { get; set; }
    public virtual Group Group { get; set; }
    
    public Schedule() {}
    public Schedule(string weekType, TimeSlot timeSlot, string dayOfWeek, Class @class, Group group)
    {
        WeekType = weekType;
        TimeSlot = timeSlot;
        DayOfWeek = dayOfWeek;
        Class = @class;
        Group = group;
    }
}