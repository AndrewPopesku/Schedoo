namespace Schedoo.Server.Models;

public class ScheduleDate
{
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    public DateTime Date { get; set; }
    
    public virtual Schedule Schedule { get; set; }
}