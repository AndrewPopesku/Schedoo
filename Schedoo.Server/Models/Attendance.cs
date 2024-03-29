namespace Schedoo.Server.Models;

public enum AttendanceStatus
{
    Present,
    Absent,
}

public class Attendance
{
    public int Id { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
    public int ScheduleDateId { get; set; }
    public string StudentId { get; set; }
    public string StudentFullName { get; set; }
    
    public virtual ScheduleDate ScheduleDate { get; set; }
}