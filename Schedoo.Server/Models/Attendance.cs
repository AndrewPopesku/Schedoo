namespace Schedoo.Server.Models;

public enum AttendanceStatus
{
    Present,
    Absent
}

public class Attendance
{
    public int Id { get; set; }
    public ScheduleDate ScheduleDate { get; set; }
    public User Student { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
}