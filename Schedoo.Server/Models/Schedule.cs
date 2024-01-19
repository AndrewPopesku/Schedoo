﻿namespace Schedoo.Server.Models;

public class Schedule
{
    public int Id { get; set; }
    public string WeekType { get; set; }
    public int TimeSlotId { get; set; }
    public string DayOfWeek { get; set; }
    public Guid ClassId { get; set; }
    public string GroupId { get; set; }

    public virtual TimeSlot? TimeSlot { get; set; }
    public virtual Class? Class { get; set; }
    
    public Schedule() {}
    public Schedule(string weekType, TimeSlot timeSlot, string dayOfWeek, Class @class)
    {
        WeekType = weekType;
        TimeSlot = timeSlot;
        DayOfWeek = dayOfWeek;
        Class = @class;
    }

    public Schedule(Schedule s)
    {
        Id = s.Id;
        WeekType = s.WeekType;
        TimeSlotId = s.TimeSlotId;
        DayOfWeek = s.DayOfWeek;
        ClassId = s.ClassId;
        GroupId = s.GroupId;
    }
}