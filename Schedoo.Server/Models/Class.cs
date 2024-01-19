namespace Schedoo.Server.Models;

public class Class
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LessonType { get; set; }
    public string LinkToMeeting { get; set; }
    public Guid RoomId { get; set; }
    public Guid TeacherId { get; set; }
    
    public virtual Room Room { get; set; }
    public virtual Teacher Teacher { get; set; }
}