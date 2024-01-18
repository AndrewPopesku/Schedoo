namespace Schedoo.Server.Models;

public class Class
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LessonType { get; set; }
    public string LinkToMeeting { get; set; }
    public int RoomId { get; set; }
    public int TeacherId { get; set; }
    
    public virtual Room Room { get; set; }
    public virtual Teacher Teacher { get; set; }
}